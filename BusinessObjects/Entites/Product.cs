using BusinessObjects.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Entites;

public class Product

{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageURL { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; }

    public ICollection<CartItem> CartItems { get; set; }
}
