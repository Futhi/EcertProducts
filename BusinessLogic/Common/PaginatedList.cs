using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Common
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }

        public PaginatedList(List<T> items, int count, int pageindex, int pagesize)
        {
            PageIndex = pageindex;
            TotalPages = (int)Math.Ceiling(count / (double)pagesize);
            this.AddRange(items);
        }

        public bool PreviousPage
        {
            get { return this.PageIndex > 1; }
        }

        public bool NextPage
        {
            get { return this.PageIndex < TotalPages; }
        }

        public static  PaginatedList<T> CreateAsync(List<T> source, int pageIndex, int pageSize)
        {
            var count =  source.Count();//table data count
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }

    }
}
