using BusinessObjects.Entites;
using DataAccess.DTOs.Cart;
using DataAccess.DTOs.CartItem;
using DataAccess.DTOs.Category;
using DataAccess.DTOs.Product;
using DataAccess.Irepository;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.Security.Claims;

namespace WebAPI.Controllers;

[Route("odata/Carts")]
[ApiController]
public class CartController : ODataController
{
    private readonly ICartRepository repository;

    public CartController(ICartRepository repository)
    {
        this.repository = repository;
    }

    [Authorize]
    [HttpGet]
    [EnableQuery]
    public async Task<IActionResult> Get()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdString, out int userId))
        {
            return BadRequest("Invalid user ID format.");
        }

        var cart = await repository.GetByUserIdAsync(userId);
        if (cart == null)
        {
            return NotFound("Cart not found.");
        }

        var cartDto = new CartDTO
        {
            Id = cart.Id,
            UserId = cart.UserId,
            CartItems = cart.CartItems?.Select(ci => new CartItemDTO
            {
                Id = ci.Id,
                ProductId = ci.ProductId,
                Quantity = ci.Quantity,
                Product = ci.Product != null ? new GETProduct
                {
                    Id = ci.Product.Id,
                    Name = ci.Product.Name,
                    Price = ci.Product.Price,
                    Description = ci.Product.Description,
                    ImageURL = ci.Product.ImageURL,
                    CategoryId = ci.Product.CategoryId,
                    Category = ci.Product.Category != null ? new GETCategory
                    {
                        Id = ci.Product.Category.Id,
                        Name = ci.Product.Category.Name
                    } : null
                } : null
            }).ToList() ?? new List<CartItemDTO>()
        };

        return Ok(cartDto);
    }

    [Authorize]
    [HttpPost("add")]
    public async Task<IActionResult> AddItem([FromBody] AddCartItemDTO cartItemDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdString, out int userId))
        {
            return BadRequest("Invalid user ID format.");
        }

        var cart = await repository.GetByUserIdAsync(userId);
        if (cart == null)
        {
            cart = new Cart { UserId = userId, CartItems = new List<CartItem>() };
            await repository.AddCartAsync(cart);
        }

        var cartItem = new CartItem
        {
            //CartId = cartItemDto.CartId,
            CartId = cart.Id,
            ProductId = cartItemDto.ProductId,
            Quantity = cartItemDto.Quantity,
        };

        await repository.AddItemAsync(cartItem);
        return Ok();
    }

    [Authorize]
    [HttpPost("remove/{cartItemId}")]
    public async Task<IActionResult> RemoveItem(int cartItemId)
    {
        await repository.RemoveItemAsync(cartItemId);
        return Ok();
    }

    //[Authorize]
    //[HttpPost("clear")]
    //public async Task<IActionResult> ClearCart([FromBody] int cartId)
    //{
    //    await repository.ClearCartAsync(cartId);
    //    return Ok();
    //}
    [Authorize]
    [HttpPost("clearAll")]
    public async Task<IActionResult> ClearAllItems()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdString, out int userId))
        {
            return BadRequest("Invalid user ID format.");
        }

        await repository.ClearCartAsync(userId);
        return Ok();
    }
}
