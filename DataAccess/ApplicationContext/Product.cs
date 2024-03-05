using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataAccess.ApplicationContext
{
    public class Product
    {
        public int Id { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public string ProductCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get; set; }
        public string Image { get; set; }
        public DateTime UpdateDate { get; set; }

        // Navigation property
        public virtual Category Category { get; set; }
    }
}
