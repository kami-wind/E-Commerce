using System.Net;
using System.Security.Cryptography;
using System.Text;
using WebApi.Helpers.AppExceptions;
using WebApi.Models;
using WebApi.Options;

namespace WebApi.Helpers
{
    public class VnpayHelper
    {
        private readonly SortedList<string, string> _requestUrlParameters;
        private readonly SortedList<string, string> _responseData;
        private readonly string _baseUrl;

        /// <summary>
        ///     Create a new VnpayHelper instance with given <paramref name="baseUrl"/>.
        /// </summary>
        /// <param name="baseUrl">
        ///     This url will be later used to build the payment url that redirect to VNPAY.
        /// </param>
        public VnpayHelper(string baseUrl)
        {
            _baseUrl = baseUrl;

            _requestUrlParameters = new SortedList<string, string>();
            _responseData = new SortedList<string, string>();
        }

        #region Request section.
        /// <summary>
        ///     Add the parameter with specify key and value to the request url.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public VnpayHelper AddRequestParameter(string key, string value)
        {
            // Avoid empty key & value.
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(key));
            }

            // Prevent to add the duplicate key & value.
            if (_requestUrlParameters.ContainsKey(key))
            {
                return this;
            }

            _requestUrlParameters.Add(key, value);

            return this;
        }

        /// <summary>
        ///     Build the request url that sent to VNPAY api with specified <paramref name="vnpayOptions"/>.
        /// </summary>
        /// <param name="vnpayOptions"></param>
        /// <returns>
        ///     The request url.
        /// </returns>
        public string BuildPaymentUrl(VnpayOptions vnpayOptions)
        {
            if (_requestUrlParameters.Count == 0)
            {
                throw new BuildUrlException("Cannot build payment due to request url parameter list is empty.");
            }

            var queryStringBuilder = new StringBuilder();

            foreach (var parameter in _requestUrlParameters)
            {
                var queryStringParameter = $"{parameter.Key}={WebUtility.UrlEncode(parameter.Value)}&";

                queryStringBuilder.Append(queryStringParameter);
            }

            // Get the query string that build from request builder
            // to generate the vnpay secure hash.
            var signData = queryStringBuilder.ToString(); ;

            if (signData.Length > 0)
            {
                signData = signData.Remove(queryStringBuilder.Length - 1, 1);
            }

            var vnp_SecureHash = HmacSHA512(vnpayOptions.HashSecret, signData);

            // Append the secure hash to request builder.
            queryStringBuilder.Append($"vnp_SecureHash={vnp_SecureHash}");

            return _baseUrl + "?" + queryStringBuilder.ToString();
        }

        private static string HmacSHA512(string key, string inputData)
        {
            var hash = new StringBuilder();
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var inputBytes = Encoding.UTF8.GetBytes(inputData);
            using (var hmac = new HMACSHA512(keyBytes))
            {
                var hashValue = hmac.ComputeHash(inputBytes);
                foreach (var theByte in hashValue)
                {
                    hash.Append(theByte.ToString("x2"));
                }
            }

            return hash.ToString();
        }
        #endregion

        #region Response section
        public VnpayHelper AddResponseData(string key, string value)
        {
            // Avoid empty key & value.
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
            {
                return this;
            }

            // Prevent to add the duplicate key & value.
            if (_responseData.ContainsKey(key))
            {
                return this;
            }

            _responseData.Add(key, value);

            return this;
        }

        public string GetResponseData(string key)
        {
            return _responseData.GetValueOrDefault(key);
        }

        /// <summary>
        ///     The raw format that exactly same as the received query string from VNPAY
        /// </summary>
        /// <returns></returns>
        private string GetResponseDataRawFormat()
        {
            StringBuilder data = new();
            if (_responseData.ContainsKey("vnp_SecureHashType"))
            {
                _responseData.Remove("vnp_SecureHashType");
            }

            if (_responseData.ContainsKey("vnp_SecureHash"))
            {
                _responseData.Remove("vnp_SecureHash");
            }

            foreach (KeyValuePair<string, string> kv in _responseData)
            {
                if (!String.IsNullOrEmpty(kv.Value))
                {
                    data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
                }
            }

            //remove last '&'
            if (data.Length > 0)
            {
                data.Remove(data.Length - 1, 1);
            }
            return data.ToString();
        }

        public bool VerifySecureHash(string secureHash, VnpayOptions vnpayOptions)
        {
            var responseDataRawFormat = GetResponseDataRawFormat();

            var validSecureHash = HmacSHA512(vnpayOptions.HashSecret, responseDataRawFormat);

            return validSecureHash.Equals(secureHash, StringComparison.InvariantCultureIgnoreCase);
        }

        public VnpayResponse GetVnpayResponse(HttpContext context)
        {
            var responseQueryString = context.Request.Query;

            if (responseQueryString.Count == 0)
            {
                return new VnpayResponse
                {
                    Success = false,
                };
            }

            foreach (var dataItem in responseQueryString)
            {
                if (dataItem.Key.StartsWith(VnpayParameters.VnpayPrefix))
                {
                    AddResponseData(dataItem.Key, dataItem.Value);
                }
            }

            var transactionReferenceNumber = GetResponseData(VnpayParameters.VnpayTransactionReferenceNumber);
            var transactionId = GetResponseData(VnpayParameters.VnpayTransactionNo);
            var responseCode = GetResponseData(VnpayParameters.VnpayResponseCode);
            var secureHash = GetResponseData(VnpayParameters.VnpaySecureHash);
            var orderInfo = GetResponseData(VnpayParameters.VnpayOrderInfo);

            return new VnpayResponse
            {
                Success = true,
                TransactionId = transactionId,
                TransactionReferenceNumber = transactionReferenceNumber,
                ResponseCode = responseCode,
                SecureHash = secureHash,
                OrderInfo = orderInfo
            };
        }
        #endregion
    }
}
