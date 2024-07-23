using DataAccess.DTOs.CartItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs.Cart;

public class CartDTO
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public List<CartItemDTO> CartItems { get; set; }
}
