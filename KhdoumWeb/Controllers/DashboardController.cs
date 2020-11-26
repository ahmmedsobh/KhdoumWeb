using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KhdoumWeb.Data;
using KhdoumWeb.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace KhdoumWeb.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            this._context = context;
        }
        public IActionResult Index()
        {
            return View(dashboard());
        }

        public DashboardModelView dashboard()
        {
            var db = new DashboardModelView();

            // Categoreis
            db.CategoriesCount =  _context.Categories.Count();
            db.ActiveCategories = _context.Categories.Where(c => c.State).Count();
            db.UnActiveCategories = _context.Categories.Where(c => c.State == false).Count();

            // SubCategories
            db.SubCategoriesCount = _context.SubCategories.Count();
            db.ActiveSubCategories = _context.SubCategories.Where(c => c.State).Count();
            db.UnActiveSubCategories = _context.SubCategories.Where(c => c.State == false).Count();

            //Members
            db.MembersCount = _context.Members.Count();
            db.ActiveMembers = _context.Members.Where(c => c.State).Count();
            db.UnActiveMembers = _context.Members.Where(c => c.State == false).Count();

            //Items
            db.ItemsCount = _context.Items.Count();
            db.ActiveItems = _context.Items.Where(c => c.State).Count();
            db.UnActiveItems = _context.Items.Where(c => c.State == false).Count();

            return db;
        }
    }
}
