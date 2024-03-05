using Dasync.Collections;
using DataAccess.ApplicationContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models
{
    public class ProductModel
    {
        public ProductModel()
        {
           
            AvailableCategories = new List<Category>();
            ProductCode = string.Empty;
            Name= string.Empty;
            Description= string.Empty;  
            CategoryName= string.Empty;
         //   picture = new FormFile();
        }

        public int Id { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public string ProductCode { get; set; }
        [Required]
        [DisplayName("Product Name")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        [Required]
        [Precision(18, 2)]
        public decimal Price { get; set; }
        public IFormFile? picture { get; set; }
        [Required]
        public DateTime UpdateDate { get; set; }

        public List<Category> AvailableCategories { get; set; }
    }
}
