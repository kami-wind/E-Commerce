using BusinessObjects.Entites;
using DataAccess.DTOs.Category;
using DataAccess.DTOs.Product;
using DataAccess.Irepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers;

//[Authorize(Roles = "Admin")]
[Route("odata/Products")]
[ApiController]
public class ProductController : ODataController
{
    private readonly IProductRepository repository;

    public ProductController(IProductRepository repository)
    {
        this.repository = repository;
    }

    [HttpGet]
    [EnableQuery]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        var product = await repository.GetAllProducts();

        // Manually map Product entities to GETProduct DTOs
        var productDtos = product.Select(p => new GETProduct
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            Description = p.Description,
            ImageURL = p.ImageURL,
            CategoryId = p.CategoryId,
            Category = new GETCategory
            {
                Id = p.Category.Id,
                Name = p.Category.Name
            }
        }).ToList();

        return Ok(productDtos);
    }

    [HttpGet("{id:int}")]
    [EnableQuery]
    public async Task<ActionResult<GETProduct>> GetProductById(int id)
    {
        var product = await repository.GetProductById(id);

        if (product == null)
        {
            return NotFound();
        }

        var productDto = new GETProduct
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Description = product.Description,
            ImageURL = product.ImageURL,
            CategoryId = product.CategoryId,
            Category = new GETCategory
            {
                Id = product.Category.Id,
                Name = product.Category.Name
            }
        };

        return Ok(productDto);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<GETProduct>> CreateProduct([FromBody] POST_PUT_Product productDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var product = new Product
        {
            Name = productDto.Name,
            Price = productDto.Price,
            Description = productDto.Description,
            ImageURL = productDto.ImageURL,
            CategoryId = productDto.CategoryId
        };

        var createdProduct = await repository.AddProduct(product);

        return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);
    }

    [Authorize]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put(int id, [FromBody] POST_PUT_Product productDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var product = await repository.GetProductById(id);
        if (product == null)
        {
            return NotFound();
        }

        product.Name = productDto.Name;
        product.Price = productDto.Price;
        product.Description = productDto.Description;
        product.ImageURL = productDto.ImageURL;
        product.CategoryId = productDto.CategoryId;

        await repository.UpdateProduct(product);

        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var product = await repository.GetProductById(id);
        if (product == null)
        {
            return NotFound();
        }

        await repository.DeleteProduct(id);

        return NoContent();
    }
}
