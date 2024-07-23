using BusinessObjects;
using BusinessObjects.Entites;
using DataAccess.Irepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository;

public class CartRepository : ICartRepository
{
    private readonly ApplicationDbContext _context;

    public CartRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddCartAsync(Cart cart)
    {
         _context.Carts.Add(cart);
        await _context.SaveChangesAsync();
    }

    public async Task AddItemAsync(CartItem cartItem)
    {
        //var cart = await _context.Carts
        //    .Include(c => c.CartItems)
        //    .FirstOrDefaultAsync(c => c.Id == cartItem.CartId);

        //if (cart != null)
        //{
        //    cart.CartItems.Add(cartItem);
        //    await _context.SaveChangesAsync();
        //}
        var existingCartItem = await _context.CartItems
    .FirstOrDefaultAsync(ci => ci.CartId == cartItem.CartId && ci.ProductId == cartItem.ProductId);

        if (existingCartItem != null)
        {
            existingCartItem.Quantity += cartItem.Quantity;
            _context.CartItems.Update(existingCartItem);
        }
        else
        {
            await _context.CartItems.AddAsync(cartItem);
        }

        await _context.SaveChangesAsync();
    }

    public async Task ClearCartAsync(int cartId)
    {
        //var cart = await _context.Carts
        //    .Include(c => c.CartItems)
        //    .FirstOrDefaultAsync(c => c.Id == cartId);

        //if (cart != null)
        //{
        //    _context.CartItems.RemoveRange(cart.CartItems);
        //    await _context.SaveChangesAsync();
        //}
        var cartItems = _context.CartItems.Where(ci => ci.CartId == cartId);
        _context.CartItems.RemoveRange(cartItems);
        await _context.SaveChangesAsync();
    }

    public async Task<Cart> GetByUserIdAsync(int userId)
    {
        return await _context.Carts
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
            .ThenInclude(p => p.Category)
            .FirstOrDefaultAsync(c => c.UserId == userId);
    }

    public async Task RemoveItemAsync(int cartItemId)
    {
        var cartItem = await _context.CartItems.FindAsync(cartItemId);
        if (cartItem != null)
        {
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }
    }
}
