using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KhdoumWeb.Models.ViewModels
{
    public class DashboardModelView
    {
        // categories
        public int CategoriesCount { get; set; }
        public int ActiveCategories { get; set; }
        public int UnActiveCategories { get; set; }
        //subcategories
        public int SubCategoriesCount { get; set; }
        public int ActiveSubCategories { get; set; }
        public int UnActiveSubCategories { get; set; }

        //Members
        public int MembersCount { get; set; }
        public int ActiveMembers { get; set; }
        public int UnActiveMembers { get; set; }

        //Items
        public int ItemsCount { get; set; }
        public int ActiveItems { get; set; }
        public int UnActiveItems { get; set; }


    }
}
