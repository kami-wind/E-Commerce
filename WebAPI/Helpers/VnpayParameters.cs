namespace WebApi.Helpers
{
    public static class VnpayParameters
    {
        public const string VnpayPrefix = "vnp_";

        public const string VnpayVersion = "vnp_Version";

        public const string VnpayCommand = "vnp_Command";

        public const string VnpayTmnCode = "vnp_TmnCode";

        public const string VnpayAmount = "vnp_Amount";

        public const string VnpayBankCode = "vnp_BankCode";

        public const string VnpayCreatedDate = "vnp_CreateDate";

        public const string VnpayCurrencyCode = "vnp_CurrCode";

        public const string VnpayClientIpAddress = "vnp_IpAddr";

        public const string VnpayLocale = "vnp_Locale";

        /// <summary>
        ///     The description that will display on the screen when client do the payment
        ///     at the VNPAY portal.
        /// </summary>
        public const string VnpayOrderInfo = "vnp_OrderInfo";

        public const string VnpayOrderType = "vnp_OrderType";

        public const string VnpayReturnUrl = "vnp_ReturnUrl";

        public const string VnpayTransactionReferenceNumber = "vnp_TxnRef";

        public const string VnpayTransactionNo = "vnp_TransactionNo"; 

        public const string VnpayResponseCode = "vnp_ResponseCode"; 
     
        public const string VnpaySecureHash = "vnp_SecureHash"; 
    }
}
