using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApi.Helpers;
using WebApi.Models;
using WebApi.Options;
using WebAPI.Service;

namespace WebAPI.Controllers;

[Route("odata/[controller]")]
[ApiController]
public class OrderController : ODataController
{

    private readonly VnPayService _vpnpayService;
    private readonly VnpayHelper _vnpayHelper;
    private readonly VnpayOptions _vnpayOptions;

    public OrderController(VnPayService vpnpayService, VnpayOptions vnpayOptions)
    {
        _vpnpayService = vpnpayService;
        _vnpayOptions = vnpayOptions;
        _vnpayHelper = new VnpayHelper(vnpayOptions.BaseUrl);
    }

    [HttpPost]
    public IActionResult GoToPayment([FromBody] int amount)
    {
        var paymentDetail = new PaymentDetail
        {
            OrderInfo = "Thanh toan don hang",
            //TotalAmount = 100_000,
            TotalAmount = amount,
            ClientIpAddress = HttpContextHelper.GetClientIpAddress(HttpContext),
        };

        var paymentUrl = _vpnpayService.GetPaymentUrl(paymentDetail);

        return Ok(new
        {
            PaymentUrl = paymentUrl,
        });
    }

    [HttpGet("vnpay")]
    public IActionResult CheckPayment()
    {
        var response = _vnpayHelper.GetVnpayResponse(HttpContext);

        if (!response.Success)
        {
            return NoContent();
        }

        if (response.ResponseCode != VnpayResponse.PaymentSuccessCode)
        {
            return NoContent();
        }

        var isValidResponse = _vnpayHelper.VerifySecureHash(response.SecureHash, _vnpayOptions);

        if (!isValidResponse)
        {
            return BadRequest();
        }

        // Implement logic to save order to database here.

        return Ok();
    }
}
