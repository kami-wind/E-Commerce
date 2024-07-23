using BusinessObjects.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Irepository;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllProducts();

    Task<Product> GetProductById(int id);

    Task<Product> AddProduct(Product product);

    Task<Product> UpdateProduct(Product product);

    Task DeleteProduct(int id);
}
