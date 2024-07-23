using BusinessObjects.Entites;
using DataAccess.DTOs.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs.Product;

public class GETProduct
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public string ImageURL { get; set; }

    public int CategoryId { get; set; }
    public GETCategory Category { get; set; }
}
