using Microsoft.AspNetCore.Mvc;

namespace MVC.Models;

public class CartItem
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
}
