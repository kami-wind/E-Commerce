namespace WebApi.Models;

public class VnpayResponse
{
    /// <summary>
    ///     Reference document: https://sandbox.vnpayment.vn/apis/docs/truy-van-hoan-tien/querydr&refund.html
    /// </summary>
    public const string PaymentSuccessCode = "00";

    public bool Success { get; set; }

    public string TransactionId { get; set; }

    public string TransactionReferenceNumber { get; set; }

    public string ResponseCode { get; set; }

    public string OrderInfo { get; set; }

    public string SecureHash { get; set; }
}
