using BusinessObjects;
using BusinessObjects.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository;

public class PaymentRepository
{
    private readonly ApplicationDbContext dbcontext;

    public PaymentRepository(ApplicationDbContext dbcontext)
    {
        this.dbcontext = dbcontext;
    }

    public VnpayPaymentRequest ProcessPayment(int userId, decimal amount)
    {
        // Simulate payment processing logic
        var payment = new VnpayPaymentRequest
        {
            UserId = userId,
            Amount = amount,
            PaymentDate = DateTime.UtcNow,
            Status = "Success"
        };

        // Save payment to the database (implementation skipped for demo purposes)
        // Example: _dbContext.Payments.Add(payment); _dbContext.SaveChanges();

        dbcontext.vnpayPaymentRequests.Add(payment);

        dbcontext.SaveChanges();
        return payment;
    }
}
