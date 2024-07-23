namespace WebApi.Models;

public class PaymentDetail
{
    /// <summary>
    ///     The total amount of money of the order.
    /// </summary>
    public int TotalAmount { get; set; }

    public string ClientIpAddress { get; set; }

    public string OrderInfo { get; set; }
}
