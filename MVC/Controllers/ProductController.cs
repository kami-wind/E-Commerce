using DataAccess.DTOs.CartItem;
using DataAccess.DTOs.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.Service;

namespace MVC.Controllers;

public class ProductController : Controller
{

    private readonly ProductService productService;

    public CategoryService CategoryService { get; }

    public ProductController(ProductService productService, CategoryService categoryService)
    {
        this.productService = productService;
        CategoryService = categoryService;
    }

    // GET: /Product/Index
    [AllowAnonymous]
    public async Task<IActionResult> Index(string searchString)
    {
        // try
        // {
        //     var products = await productService.GetItems();
        //     return View(products);
        // }
        // catch (Exception ex)
        // {
        //     return View("Error");
        // }
        
        var products = await productService.GetItems();

        if (!string.IsNullOrEmpty(searchString))
        {
           products = products.Where(p => p.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                                          p.Description.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        return View(products);
    }

    // GET: /Product/Create
    [Authorize]
    public async Task< IActionResult> Create()
    {
        var userEmail = HttpContext.User.Identity.Name;
        if (userEmail != "admin@gmail.com")
        {
            return Forbid(); // Or any other action you prefer for unauthorized users
        }

        var categories = await CategoryService.GetItems(); // Assuming this method exists
        ViewBag.CategoryId = new SelectList(categories, "Id", "Name"); // Adjust properties as needed
        return View();
    }

    // POST: /Product/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Create(POST_PUT_Product product)
    {
        if (!ModelState.IsValid)
        {
            return View(product);
        }

        await productService.CreateProduct(product);
        return RedirectToAction(nameof(Index));
    }

    // GET: /Product/Edit/5
    [Authorize]
    public async Task<IActionResult> Edit(int id)
    {
        //var userEmail = HttpContext.User.Identity.Name;
        //if (userEmail != "admin@gmail.com")
        //{
        //    return Forbid(); // Or any other action you prefer for unauthorized users
        //}

        //var product = await productService.GetItem(id);
        //if (product == null)
        //{
        //    return NotFound();
        //}
        //return View(product);
        var userEmail = HttpContext.User.Identity.Name;
        if (userEmail != "admin@gmail.com")
        {
            return Forbid(); // Or any other action you prefer for unauthorized users
        }

        var product = await productService.GetItem(id);
        if (product == null)
        {
            return NotFound();
        }

        // Map GETProduct to POST_PUT_Product
        var productDto = new POST_PUT_Product
        {
            Name = product.Name,
            Price = product.Price,
            Description = product.Description,
            ImageURL = product.ImageURL,
            CategoryId = product.CategoryId
        };

        var categories = await CategoryService.GetItems(); // Assuming this method exists
        ViewBag.CategoryId = new SelectList(categories, "Id", "Name"); // Adjust properties as needed

        return View(productDto);
    }

    // POST: /Product/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Edit(int id, POST_PUT_Product product)
    {

        if (!ModelState.IsValid)
        {
            return View(product);
        }

        await productService.UpdateProduct(id, product);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int id)
    {
        var product = await productService.GetItem(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    // GET: /Product/Delete/5
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var userEmail = HttpContext.User.Identity.Name;
        if (userEmail != "admin@gmail.com")
        {
            return Forbid(); // Or any other action you prefer for unauthorized users
        }

        var product = await productService.GetItem(id);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    // POST: /Product/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await productService.DeleteProduct(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(int productId, int quantity)
    {
        try
        {
            var addCartItemDTO = new AddCartItemDTO
            {
                ProductId = productId,
                Quantity = quantity
            };

            await productService.AddToCart(addCartItemDTO);
            return RedirectToAction("CartDetails", "Cart");
        }
        catch (UnauthorizedAccessException)
        {

            return RedirectToAction("Index", "Home");
        }
        catch (HttpRequestException)
        {
            ModelState.AddModelError(string.Empty, "Failed to add item to cart.");
            return RedirectToAction("Index", "Home");
        }
    }
}
