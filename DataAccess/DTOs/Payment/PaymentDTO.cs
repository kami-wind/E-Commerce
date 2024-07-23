using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs.Payment;

public class PaymentDTO
{
    public int UserId { get; set; }
    public decimal Amount { get; set; }
}
