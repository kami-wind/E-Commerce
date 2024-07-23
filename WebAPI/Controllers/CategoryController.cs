using BusinessObjects.Entites;
using DataAccess.DTOs.Category;
using DataAccess.Irepository;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace WebAPI.Controllers;

[Route("odata/Categories")]
[ApiController]
public class CategoryController : ODataController
{
    private readonly ICategoryRepository repository;

    public CategoryController(ICategoryRepository repository)
    {
        this.repository = repository;
    }

    [HttpGet]
    [EnableQuery]
    public async Task<ActionResult<IEnumerable<Category>>> GetAll()
    {
        var category = await repository.GetAllCategorys();

        var categoryDto = category.Select(c => new GETCategory
        {
            Id = c.Id,
            Name = c.Name,
        }).ToList();

        return Ok(categoryDto);
    }

    [HttpGet("{id:int}")]
    [EnableQuery]
    public async Task<ActionResult<GETCategory>> GetById(int id)
    {
        var category = await repository.GetCategoryById(id);
        if (category == null)
        {
            return NotFound();  
        }

        var categoryDto = new GETCategory
        {
            Id = category.Id,
            Name = category.Name,
        };

        return Ok(category);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] POST_PUT_Category categoryDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var category = new Category
        {
            Name = categoryDto.Name,
        };

        await repository.AddCategory(category);
        return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] POST_PUT_Category categoryDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var category = await repository.GetCategoryById(id);
        if (category == null)
        {
            return NotFound();
        }

        category.Name = categoryDto.Name;

        await repository.UpdateCategory(category);
        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await repository.DeleteCategory(id);
        return NoContent();
    }
}
