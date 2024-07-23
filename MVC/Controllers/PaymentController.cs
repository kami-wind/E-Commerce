using BusinessObjects.Entites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC.Service;
using System.Security.Claims;

namespace MVC.Controllers;


public class PaymentController : Controller
{
    //private readonly PaymentService _paymentService;

    //public PaymentController(PaymentService paymentService)
    //{
    //    _paymentService = paymentService ?? throw new ArgumentNullException(nameof(paymentService));
    //}

    //[HttpPost]
    //public IActionResult ProcessPayment(decimal amount, int orderId, string orderInfo)
    //{
    //    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
    //    if (userIdClaim == null)
    //    {
    //        // Handle the case where user is not authenticated or userId is not found
    //        return Unauthorized();
    //    }

    //    var userId = userIdClaim.Value;

    //    // Proceed with payment processing using _paymentService
    //    var paymentUrl = _paymentService.CreatePaymentUrl(amount, orderId.ToString(), orderInfo, userId);

    //    return Redirect(paymentUrl);
    //}

    ////public IActionResult PaymentReturn()
    ////{
    ////    // Extract the query parameters from the request
    ////    var vnpayData = new Dictionary<string, string>();
    ////    foreach (var key in Request.Query.Keys)
    ////    {
    ////        vnpayData[key] = Request.Query[key];
    ////    }

    ////    // Verify the payment
    ////    bool isValid = _paymentService.VerifyPayment(vnpayData);

    ////    if (isValid)
    ////    {
    ////        // Payment is successful, update order status in the database
    ////        return View("Success");
    ////    }
    ////    else
    ////    {
    ////        // Payment failed or invalid, show an error message
    ////        return View("Failure");
    ////    }
    ////}


    private readonly PaymentService _paymentService;

    public PaymentController(PaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> GoToPayment(int amount)
    {

        try
        {
            var paymentUrl = await _paymentService.GetPaymentUrl(amount);
            return Redirect(paymentUrl);
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as appropriate
            return BadRequest($"An error occurred while processing the payment: {ex.Message}");
        }
    }
}
