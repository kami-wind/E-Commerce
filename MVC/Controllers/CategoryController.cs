using DataAccess.DTOs.Category;
using DataAccess.DTOs.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Service;

namespace MVC.Controllers;

public class CategoryController : Controller
{
    private readonly CategoryService service;

    public CategoryController(CategoryService service)
    {
        this.service = service;
    }


    public async Task< IActionResult> Index()
    {
        try
        {
            var categories = await service.GetItems();
            return View(categories);
        }
        catch (Exception ex)
        {
            return View("Error");
        }
    }

    // GET: /Category/Create
    [Authorize]
    public IActionResult Create()
    {
        var userEmail = HttpContext.User.Identity.Name;
        if (userEmail != "admin@gmail.com")
        {
            return Forbid(); // Or any other action you prefer for unauthorized users
        }
        return View();
    }


    // POST: /Category/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Create(POST_PUT_Category category)
    {
        if (!ModelState.IsValid)
        {
            return View(category);
        }

        await service.CreateCategory(category);
        return RedirectToAction(nameof(Index));
    }


    // GET: /Category/Edit/5
    [Authorize]
    public async Task<IActionResult> Edit(int id)
    {
        var userEmail = HttpContext.User.Identity.Name;
        if (userEmail != "admin@gmail.com")
        {
            return Forbid(); // Or any other action you prefer for unauthorized users
        }

        var category = await service.GetItem(id);
        if (category == null)
        {
            return NotFound();
        }
        return View(category);
    }


    // POST: /Category/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Edit(int id, POST_PUT_Category category)
    {

        if (!ModelState.IsValid)
        {
            return View(category);
        }

        await service.UpdateCategory(id, category);
        return RedirectToAction(nameof(Index));
    }


    public async Task<IActionResult> Details(int id)
    {
        var product = await service.GetItem(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    // GET: /Category/Delete/5
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var userEmail = HttpContext.User.Identity.Name;
        if (userEmail != "admin@gmail.com")
        {
            return Forbid(); // Or any other action you prefer for unauthorized users
        }

        var product = await service.GetItem(id);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }


    // POST: /Category/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await service.DeleteCategory(id);
        return RedirectToAction(nameof(Index));
    }
}
