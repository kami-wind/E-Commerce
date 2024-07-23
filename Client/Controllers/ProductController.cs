using Client.Service;
using DataAccess.DTOs.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers;

public class ProductController : Controller
{

    private readonly ProductService productService;

    public ProductController(ProductService productService)
    {
        this.productService = productService;
    }

    // GET: /Product/Index
    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        try
        {
            var products = await productService.GetItems();
            return View(products);
        }
        catch (Exception ex)
        {
            // Handle the exception appropriately (e.g., log it, show an error message, etc.)
            return View("Error");
        }
    }

    // GET: /Product/Create
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Product/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
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
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id)
    {
        var product = await productService.GetItem(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    // POST: /Product/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
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
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
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
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await productService.DeleteProduct(id);
        return RedirectToAction(nameof(Index));
    }
}
