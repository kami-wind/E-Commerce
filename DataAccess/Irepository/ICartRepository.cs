using BusinessObjects.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Irepository;

public interface ICartRepository
{
    Task<Cart> GetByUserIdAsync(int userId);
    Task AddCartAsync(Cart cart);
    Task AddItemAsync(CartItem cartItem);
    Task RemoveItemAsync(int cartItemId);
    Task ClearCartAsync(int cartId);
}
