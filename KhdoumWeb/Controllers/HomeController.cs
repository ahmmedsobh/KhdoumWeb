﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KhdoumWeb.Models;
using KhdoumWeb.Data;
using Microsoft.EntityFrameworkCore;
using KhdoumWeb.Models.ViewModels;

namespace KhdoumWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        //private bool IsLogin { get; set; }
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            this._context = context;
        }

        public IActionResult Index(int LoadItemsCount = 9)
        {
            return View(HomeModels(LoadItemsCount));
        }


        public IActionResult Search(string q)
        {
            var items = (from i in _context.Items.Include(i => i.Member)
                         from c in _context.Categories
                         from sc in _context.SubCategories
                         from ic in _context.ItemsCategories
                         from isc in _context.ItemsSupCategories
                         where ic.ItemId == i.Id && ic.CategoryId == c.Id && isc.ItemId == i.Id && isc.SubCategoryId == sc.Id
                         where i.Title.Contains(q) || i.Description.Contains(q) || i.Address.Contains(q) || c.Name.Contains(q) || sc.Name.Contains(q) || i.Member.FullName.Contains(q)
                         select i).ToList();

            return View(items);
        }

        public IActionResult SubCategories(int id, int SubId = 0)
        {
            if (id == 0)
            {
                return NotFound();
            }
            return View(SubCategoryModels(id, SubId));
        }

        public IActionResult Item(string id, int FromAction = 0)
        {
            if (id == null)
            {
                return NotFound();
            }


            var item = (from i in _context.Items.Include(i => i.Member)
                        from c in _context.Categories
                        from sc in _context.SubCategories
                        from ic in _context.ItemsCategories
                        from isc in _context.ItemsSupCategories
                        where ic.ItemId == i.Id && ic.CategoryId == c.Id && isc.ItemId == i.Id && isc.SubCategoryId == sc.Id && i.ShortLink == id
                        select new ItemDetailsViewModel
                        {
                            Id = i.Id
                             ,
                            Title = i.Title
                             ,
                            Date = i.Date
                             ,
                            ImgUrl = i.ImgUrl
                             ,
                            Description = i.Description
                             ,
                            Address = i.Address
                             ,
                            ShortLink = i.ShortLink
                             ,
                            ViewsCount = i.ViewsCount
                             ,
                            RatingValues = i.RatingValues
                             ,
                            RatingCount = i.RatingCount
                             ,
                            Price = i.Price
                             ,
                            IsPaid = i.IsPaid
                             ,
                            Phone = i.Phone
                             ,
                            CategoryName = c.Name
                             ,
                            SubCategoryName = sc.Name
                             ,
                            CategoryId = c.Id
                             ,
                            SubCategoryId = sc.Id
                             ,
                            Member = i.Member
                        }).FirstOrDefault();

            if (FromAction == 0)
            {
                var ItemForViewCount = _context.Items.FirstOrDefault(i => i.ShortLink == id);
                if (ItemForViewCount.ViewsCount == null)
                {
                    ItemForViewCount.ViewsCount = 0;
                }

                ItemForViewCount.ViewsCount++;
                _context.Update(ItemForViewCount);
                _context.SaveChanges();
            }


            return View(item);
        }

        public IActionResult Order(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var Item = _context.Items.FirstOrDefault(i => i.Id == id);

            if (Item == null)
            {
                return NotFound();
            }
            if (Item.ClicksCount == null)
            {
                Item.ClicksCount = 0;
            }

            Item.ClicksCount++;
            _context.Update(Item);
            _context.SaveChanges();

            return RedirectToAction(nameof(Item), new { id = Item.ShortLink, FromAction = 1 });
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Members.FirstOrDefault(m => m.Phone == login.Phone || m.Email == login.Phone && m.Password == login.Password);
                if (user == null)
                {
                    return NotFound();
                }

                TempData["IsLogin"] = "true";
                return RedirectToAction(nameof(Dashboard), new { id = user.Id });
            }
            return View(login);
        }

        public IActionResult Dashboard(int id = 0)
        {
            if (TempData["IsLogin"] as string != "true")
            {
                return RedirectToAction(nameof(Login));
            }

            if (id == 0)
            {
                return NotFound();
            }

            var member = _context.Members.Include(m => m.Items).FirstOrDefault(m => m.Id == id);
            return View(member);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public HomeModelView HomeModels(int LoadItemsCount)
        {
            HomeModelView homeModel = new HomeModelView();

            var Categories = _context.Categories.Include(c => c.ItemsCategories).Where(c => c.State).ToList();
            var Items = (from i in _context.Items
                         from c in _context.Categories
                         from sc in _context.SubCategories
                         from ic in _context.ItemsCategories
                         from isc in _context.ItemsSupCategories
                         where ic.ItemId == i.Id && ic.CategoryId == c.Id && isc.ItemId == i.Id && isc.SubCategoryId == sc.Id
                         where i.State
                         select i).ToList().TakeLast(LoadItemsCount).ToList();




            homeModel.Categories = Categories;
            homeModel.Items = Items;

            return homeModel;
        }

        public HomeModelView SubCategoryModels(int id, int SubId)
        {
            HomeModelView SubCategoryModels = new HomeModelView();

            var SubCategories = (from i in _context.Items
                                 from c in _context.Categories
                                 from sc in _context.SubCategories
                                 from ic in _context.ItemsCategories
                                 from isc in _context.ItemsSupCategories
                                 where ic.ItemId == i.Id && ic.CategoryId == c.Id && isc.ItemId == i.Id && isc.SubCategoryId == sc.Id && c.Id == id
                                 select sc).ToList();

            var Items = new List<Item>();

            if (SubId != 0)
            {
                Items = (from i in _context.Items
                         from c in _context.SubCategories
                         from isc in _context.ItemsSupCategories
                         where isc.ItemId == i.Id && isc.SubCategoryId == c.Id && c.Id == SubId
                         select i).ToList();
            }
            else
            {
                Items = (from i in _context.Items
                         from c in _context.Categories
                         from ic in _context.ItemsCategories
                         where ic.ItemId == i.Id && ic.CategoryId == c.Id && c.Id == id
                         select i).ToList();
            }


            SubCategoryModels.SubCategories = SubCategories;
            SubCategoryModels.Items = Items;
            SubCategoryModels.CategoryId = id;
            return SubCategoryModels;
        }
    }
}