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

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext dbcontext;

    public CategoryRepository(ApplicationDbContext dbcontext)
    {
        this.dbcontext = dbcontext;
    }


    public async Task<Category> AddCategory(Category category)
    {
        dbcontext.Categories.Add(category);

        await dbcontext.SaveChangesAsync();

        return category;
    }

    public async Task DeleteCategory(int id)
    {
        var category = await dbcontext.Categories.FindAsync(id);
        if (category != null)
        {
            dbcontext.Categories.Remove(category);

            await dbcontext.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Category>> GetAllCategorys()
    {
        return await dbcontext.Categories.ToListAsync();
    }

    public async Task<Category> GetCategoryById(int id)
    {
        return await dbcontext.Categories.FindAsync(id);
    }

    public async Task<Category> UpdateCategory(Category category)
    {
        dbcontext.Categories.Update(category);

        await dbcontext.SaveChangesAsync();

        return category;
    }
}
