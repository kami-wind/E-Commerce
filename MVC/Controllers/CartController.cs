using BusinessObjects.Entites;
using DataAccess.DTOs.CartItem;
using Microsoft.AspNetCore.Mvc;
using MVC.Service;
using System.Security.Claims;

namespace MVC.Controllers;

public class CartController : Controller
{
    private readonly CartService service;

    public CartController(CartService cartService, ProductService productService)
    {
        this.service = cartService;
        ProductService = productService;
    }

    public ProductService ProductService { get; }


    public async Task<IActionResult> CartDetails()
    {
        //var cart = await ProductService.GetCart();
        //return View(cart);
        try
        {
            var cart = await ProductService.GetCart();
            return View(cart);
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                // Redirect to login page if unauthorized
                return RedirectToAction("Login", "Account");
            }
            throw; // Re-throw if the exception is not handled
        }


    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveItem(int cartItemId)
    {
        try
        {
            await service.RemoveCartItemAsync(cartItemId);
            return RedirectToAction("CartDetails");
        }
        catch (Exception ex)
        {
            // Handle exception as needed
            ModelState.AddModelError("", $"Failed to remove item: {ex.Message}");
            return RedirectToAction("CartDetails");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ClearAllCartItems()
    {
        try
        {
            await service.ClearAllCartItems();
            return RedirectToAction("Index","Product");
        }
        catch (Exception ex)
        {
            // Handle exception as needed
            ModelState.AddModelError("", $"Failed to clear all items: {ex.Message}");
            return RedirectToAction("CartDetails");
        }
    }


}
