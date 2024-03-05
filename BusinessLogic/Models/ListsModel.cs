using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BusinessLogic.Models
{
    public class ListsModel
    {
        public IList<SelectListItem> AvailableCategories { get; set; }
        public IList<SelectListItem> AvailableProducts { get; set; }

        public ListsModel()
        {
            AvailableCategories = new List<SelectListItem>();
            AvailableProducts = new List<SelectListItem>();
        }
    }
}
