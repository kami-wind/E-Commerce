using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs.Product;

public class POST_PUT_Product
{
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Name is required")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Name is required")]
    public string ImageURL { get; set; }

    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    [Range (0, int.MaxValue)]
    public int Quantity { get; set; }

    public int CategoryId { get; set; }

}
