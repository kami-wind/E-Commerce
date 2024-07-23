using BusinessObjects.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Irepository;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllCategorys();

    Task<Category> GetCategoryById(int id);

    Task<Category> AddCategory(Category category);

    Task<Category> UpdateCategory(Category category);

    Task DeleteCategory(int id);
}
