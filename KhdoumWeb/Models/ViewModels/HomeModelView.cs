using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KhdoumWeb.Models.ViewModels
{
    public class HomeModelView
    {
        public List<Category> Categories { get; set; }
        public List<Item> Items { get; set; }
        public List<SubCategory> SubCategories { get; set; }


        public int CategoryId { get; set; }
    }
}
