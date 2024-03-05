using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ApplicationContext
{
    public class Category
    {
       
            public int Id { get; set; }
            public string Name { get; set; }
            public bool IsActive { get; set; }
            public DateTime UpdateDate { get; set; }
            public string CategoryCode { get; set; }
        
    }
}
