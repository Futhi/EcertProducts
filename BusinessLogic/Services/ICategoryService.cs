using BusinessLogic.Models;
using DataAccess.ApplicationContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public interface ICategoryService
    {
         Task DeleteCategory(int categoryId);
         Task UpdateCategory(CategoryModel updatedcategory);
         Task<Category> GetCategoryById(int categoryId);
        IEnumerable<Category> GetAllCategorys();
        Task CreateCategory(CategoryModel model);
    }
}
