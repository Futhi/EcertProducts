using BusinessLogic.Models;
using DataAccess.ApplicationContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class CategoryService:ICategoryService
    {
        private readonly ApplicationDBContext _dbContext;
        private IAuditService _auditService;



        public CategoryService(ApplicationDBContext context, IAuditService service)
        {
            _dbContext = context;
            _auditService = service;
        }

        public async Task CreateCategory(CategoryModel model)
        {
            await _dbContext.Categorys.FirstOrDefaultAsync(x => x.CategoryCode.ToLower() == model.CategoryCode.ToLower());
           
            Category category = new Category()
            {
                IsActive = model.IsActive,
                Name = model.Name,
                CategoryCode = model.CategoryCode,
                UpdateDate = DateTime.Now
            };

         await   _dbContext.Categorys.AddAsync(category);
         await   _dbContext.SaveChangesAsync();

            //logging
            AuditModel auditModel = new AuditModel()
            {
                TableName = "Category",
                Action = "Insert",
                RecordId = category.Id.ToString(),
                OldValue = null,
                NewValue = category.Name,
                AuditDate = DateTime.Now
            };
            _auditService.CreateAudit(auditModel);
        }

        public IEnumerable<Category> GetAllCategorys()
        {
            var results = _dbContext.Categorys.ToList();
            return results;
        }

     
        public async Task<Category> GetCategoryById(int categoryid)
        {
            var results = await _dbContext.Categorys.FirstOrDefaultAsync(p => p.Id == categoryid);

            if (results != null)
            {
                return results;
            }
            else
                return new Category();
            
        }

        // Update a product
        public async Task UpdateCategory(CategoryModel updatedCategory)
        {
            var existingCategory = await _dbContext.Categorys.FirstOrDefaultAsync(p => p.Id == updatedCategory.Id);

            if (existingCategory != null)
            {
                existingCategory.Name = updatedCategory.Name;
                existingCategory.CategoryCode = updatedCategory.CategoryCode;
                existingCategory.IsActive = updatedCategory.IsActive;
                existingCategory.UpdateDate = DateTime.Now;
            }
            await _dbContext.SaveChangesAsync();

            //logging
            AuditModel auditModel = new AuditModel()
            {
                TableName = "Category",
                Action = "Update",
                RecordId = updatedCategory.Id.ToString(),
                OldValue = null,
                NewValue = updatedCategory.Name,
                AuditDate = DateTime.Now
            };
            _auditService.CreateAudit(auditModel);
        }

        // Delete a category by ID
        public async Task DeleteCategory(int id)
        {
            var categoryToRemove = await _dbContext.Categorys.FirstOrDefaultAsync(p => p.Id == id);

            if (categoryToRemove != null)
            {
                _dbContext.Categorys.Remove(categoryToRemove);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception($"Category with ID {id} not found.");
            }

            AuditModel auditModel = new AuditModel()
            {
                TableName = "Category",
                Action = "Delete", 
                RecordId = categoryToRemove.Id.ToString(),
                OldValue = null,
                NewValue = categoryToRemove.Name,
                AuditDate = DateTime.Now
            };
            _auditService.CreateAudit(auditModel);
        }
    }
}
