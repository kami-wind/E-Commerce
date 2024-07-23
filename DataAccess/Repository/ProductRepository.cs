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

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext dbcontext;

    public ProductRepository(ApplicationDbContext dbcontext)
    {
        this.dbcontext = dbcontext;
    }


    public async Task<Product> AddProduct(Product product)
    {
        dbcontext.Products.AddAsync(product);

        await dbcontext.SaveChangesAsync();

        return product;
    }

    public async Task DeleteProduct(int id)
    {
        var product = await dbcontext.Products.FindAsync(id);
        if (product != null)
        {
            dbcontext.Products.Remove(product);

            await dbcontext.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Product>> GetAllProducts()
    {
        return await dbcontext.Products
            .Include(p => p.Category)
            .ToListAsync();
    }

    public async Task<Product> GetProductById(int id)
    {
        return await dbcontext.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Product> UpdateProduct(Product product)
    {
        var findProduct = await dbcontext.Products.FindAsync(product.Id);

        if (findProduct != null)
        {
            dbcontext.Products.Update(findProduct);

            await dbcontext.SaveChangesAsync();
        }

        return product;
    }
}
