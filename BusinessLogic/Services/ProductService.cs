using BusinessLogic.Models;
using Dasync.Collections;
using DataAccess.ApplicationContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BusinessLogic.Services
{
    public class ProductService:IProductService
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly ApplicationDBContext _categorydbContext;
        private List<Product> products;
        private IAuditService _auditService;
        private ICategoryService _categoryService;
       // private ProductModel _productModel;

        public ProductService(ApplicationDBContext context,IAuditService service, ICategoryService catService, ApplicationDBContext contextNext)
        {
            // Initialize an empty list of products
            products = new List<Product>();
            _dbContext = context;
            _auditService = service;
         //  _productModel= pmodel;
            _categoryService = catService;
            _categorydbContext= contextNext;
        }

        // Create a new product
        public async Task CreateProduct(ProductModel model,string filename)
        {
  
           var category = _dbContext.Categorys.FirstOrDefault(p => p.Id == model.CategoryId);

            if (category == null) { category = new Category(); }

           Product product = new Product()
            {
                CategoryId = model.CategoryId,
                ProductCode = model.ProductCode,
                Name = model.Name,
                Description = model.Description,
                CategoryName = model.CategoryName == "" ? model.CategoryName = category.Name : string.Empty ,
                Price = model.Price,
                Image = filename,
                UpdateDate = DateTime.Now
            };

           await _dbContext.Products.AddAsync(product);
           await _dbContext.SaveChangesAsync();

            //logging
            AuditModel auditModel = new AuditModel()
            {
                TableName = "Product",
                Action = "Insert",
                RecordId = product.Id.ToString(),
                OldValue = null,
                NewValue = product.Name,
                AuditDate = DateTime.Now
            };
            _auditService.CreateAudit(auditModel);
        }

        public async Task<List<Product>> GetAllProducts()
        {

            //   List<Product> products =new List<Product>();

            //products =  _dbContext.Products.ToList();

            var result = await _dbContext.Products.ToListAsync();
            return result;
            //await   foreach (var product in products)
            //{
            //    yield return product;
            //}
        }


        public async Task<List<Category>> GetAllAvailableCategories()
        {
            return await _categorydbContext.Categorys.ToListAsync();
            //   var categories =  _categorydbContext.Categorys.ToList();

            //await   foreach (var category in categories)
            //   {
            //       yield return category;
            //   }


        }

        // Read a product by ID
        public async Task<Product> GetProductById(int productId)
        {
            var results = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == productId);

            if (results == null)
                return new Product();
            else
            return results;
        }

        // Update a product
        public async Task UpdateProduct(ProductModel updatedProduct,string filename)
        {
            var existingProduct = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == updatedProduct.Id);
            var categoryName = "";
            if (updatedProduct.CategoryId > 0)
            {
              var   category = _dbContext.Categorys.FirstOrDefault(p => p.Id == updatedProduct.CategoryId);
                categoryName = category.Name;
            }
            

            if (existingProduct != null)
            {
                existingProduct.ProductCode = updatedProduct.ProductCode;
                existingProduct.Name  = updatedProduct.Name;
                existingProduct.Description = updatedProduct.Description;
                existingProduct.CategoryName = updatedProduct.CategoryName == "" ?categoryName  :updatedProduct.CategoryName ;
                existingProduct.Price = updatedProduct.Price;
                existingProduct.Image = filename;
                existingProduct.UpdateDate = DateTime.Now;
            }
          await _dbContext.SaveChangesAsync();

            //logging
            AuditModel auditModel = new AuditModel()
            {
                TableName = "Product",
                Action = "Update",
                RecordId = updatedProduct.Id.ToString(),
                OldValue = null,
                NewValue = updatedProduct.Name,
                AuditDate = DateTime.Now
            };
            _auditService.CreateAudit(auditModel);
        }

        // Delete a product by ID
        public async Task DeleteProduct(int productId)
        {
            var productToRemove =await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == productId);

            if (productToRemove != null)
            {
                _dbContext.Products.Remove(productToRemove);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception($"Product with ID {productId} not found.");
            }

            AuditModel auditModel = new AuditModel()
            {
                TableName = "Product",
                Action = "Delete",
                RecordId = productToRemove.Id.ToString(),
                OldValue = null,
                NewValue = productToRemove.Name,
                AuditDate = DateTime.Now
            };
            _auditService.CreateAudit(auditModel);
        }
    }
}
