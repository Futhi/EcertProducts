using BusinessLogic.Models;
using DataAccess.ApplicationContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public interface IProductService
    {
        public  Task DeleteProduct(int productId);
        public Task UpdateProduct(ProductModel updatedProduct, string filename);
        public Task<Product> GetProductById(int productId);
        public Task<List<Product>> GetAllProducts();
        public Task CreateProduct(ProductModel model,string filename);
        public Task<List<Category>> GetAllAvailableCategories();
    }
}
