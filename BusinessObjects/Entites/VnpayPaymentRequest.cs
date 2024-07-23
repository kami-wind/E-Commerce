using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Entites;

public class VnpayPaymentRequest
{
    //public int Id { get; set; }

    //public string OrderId { get; set; }
    //public string OrderInfo { get; set; }
    //public decimal Amount { get; set; }

    public int Id { get; set; }

    public int UserId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string Status { get; set; }
}
