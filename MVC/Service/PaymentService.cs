using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace MVC.Service;

public class PaymentService
{
    private readonly HttpClient _httpClient;

    public PaymentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetPaymentUrl(int amount)
    {
        // Convert amount to the smallest unit required by VNPay
        //var amountInSmallestUnit = amount * 1000;25,332
        var amountInSmallestUnit = amount * 25332;
        // Make the request to your API with the correctly formatted amount
        var response = await _httpClient.PostAsJsonAsync("odata/Order", amountInSmallestUnit);
        response.EnsureSuccessStatusCode();

        // Read and parse the response
        var responseBody = await response.Content.ReadAsStringAsync();
        var jsonResponse = JObject.Parse(responseBody);

        // Extract and return the payment URL
        if (jsonResponse.TryGetValue("paymentUrl", out var paymentUrl))
        {
            return paymentUrl.ToString();
        }

        throw new NullReferenceException("Payment URL not found in the response.");
    }

}
