using WebApi.Models;
using WebApi.Helpers;
using WebApi.Options;

namespace WebAPI.Service;

public class VnPayService
{
    private readonly VnpayHelper _vnpayHelper;
    private readonly VnpayOptions _vnpayOptions;

    public VnPayService(VnpayOptions vnpayOptions)
    {
        _vnpayOptions = vnpayOptions;
        _vnpayHelper = new VnpayHelper(vnpayOptions.BaseUrl);
    }

    public string GetPaymentUrl(PaymentDetail paymentDetail)
    {
        var createdDate = DateTime.Now;
        var transactionReferenceNumber = DateTime.Now.Ticks;

        // VNPAY api required parameter section.
        _vnpayHelper.AddRequestParameter(VnpayParameters.VnpayVersion, _vnpayOptions.Version);
        _vnpayHelper.AddRequestParameter(VnpayParameters.VnpayCommand, _vnpayOptions.Command);
        _vnpayHelper.AddRequestParameter(VnpayParameters.VnpayTmnCode, _vnpayOptions.TmnCode);

        // Order meta data section.
        var amountString = $"{paymentDetail.TotalAmount * 100}";
        _vnpayHelper.AddRequestParameter(VnpayParameters.VnpayAmount, amountString);
        _vnpayHelper.AddRequestParameter(VnpayParameters.VnpayCreatedDate, createdDate.ToString("yyyyMMddHHmmss"));
        _vnpayHelper.AddRequestParameter(VnpayParameters.VnpayCurrencyCode, _vnpayOptions.CurrencyCode);
        _vnpayHelper.AddRequestParameter(VnpayParameters.VnpayClientIpAddress, paymentDetail.ClientIpAddress);
        _vnpayHelper.AddRequestParameter(VnpayParameters.VnpayLocale, _vnpayOptions.Locale);

        // Order information section.
        _vnpayHelper.AddRequestParameter(VnpayParameters.VnpayOrderInfo, paymentDetail.OrderInfo);
        _vnpayHelper.AddRequestParameter(VnpayParameters.VnpayOrderType, "other");
        _vnpayHelper.AddRequestParameter(VnpayParameters.VnpayTransactionReferenceNumber, transactionReferenceNumber.ToString());

        _vnpayHelper.AddRequestParameter(VnpayParameters.VnpayReturnUrl, _vnpayOptions.ReturnUrl);

        return _vnpayHelper.BuildPaymentUrl(_vnpayOptions);
    }
}
