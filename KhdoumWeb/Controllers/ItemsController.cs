using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KhdoumWeb.Data;
using KhdoumWeb.Models;
using AutoMapper;
using KhdoumWeb.Helpers;
using KhdoumWeb.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace KhdoumWeb.Controllers
{
    [Authorize]
    public class ItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;
        private readonly UploadImages uploadImages;

        public ItemsController(ApplicationDbContext context, IMapper mapper, UploadImages uploadImages)
        {
            _context = context;
            this.mapper = mapper;
            this.uploadImages = uploadImages;
            uploadImages.Folder = "Items";
        }

        // GET: Items
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Items.Include(i => i.City).Include(i => i.Member);
            ViewBag.Categories = _context.Categories.Include(c => c.ItemsCategories).ToList();
            ViewBag.SupCategories = _context.SubCategories.Include(c => c.ItemsSupCategories).ToList();


            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.City)
                .Include(i => i.Member)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Items/Create
        public IActionResult Create()
        {
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name");
            ViewData["MemberId"] = new SelectList(supliers(), "Id", "FullName");
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ItemViewModel item)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var Item = mapper.Map<Item>(item);
                    Item.ImgUrl = uploadImages.AddImage(item.File);
                    Item.Sorting = _context.Items.Count() + 1;
                    Item.ShortLink = RandomURL.GetURL();
                    Item.Date = DateTime.Now.ToShortDateString();
                    if (Item.RatingValues == null)
                    {
                        Item.RatingValues = 0;
                    }
                    if (Item.RatingCount == null)
                    {
                        Item.RatingCount = 0;
                    }

                    if (item.RaitingValue != 0)
                    {
                        Item.RatingCount++;
                        Item.RatingValues += item.RaitingValue;
                    }



                    _context.Add(Item);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name", item.CityId);
                ViewData["MemberId"] = new SelectList(supliers(), "Id", "FullName", item.MemberId);
                return View(item);
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
           
        }

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var item = await _context.Items.FindAsync(id);
                if (item == null)
                {
                    return NotFound();
                }
                var Item = mapper.Map<ItemViewModel>(item);
                ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name", item.CityId);
                ViewData["MemberId"] = new SelectList(supliers(), "Id", "FullName", item.MemberId);
                return View(Item);
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
            
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ItemViewModel item)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var Item = _context.Items.AsNoTracking().FirstOrDefault(i => i.Id == id);

                        item.ClicksCount = Item.ClicksCount;
                        item.ViewsCount = Item.ViewsCount;
                        item.FavoriteCount = Item.FavoriteCount;
                        item.MembersCount = item.MembersCount;
                        item.RatingCount = item.RatingCount;
                        item.RatingValues = item.RatingValues;
                        item.ShortLink = Item.ShortLink;
                        item.Sorting = Item.Sorting;
                        item.Date = Item.Date;
                        string ImgUrl = Item.ImgUrl;
                        item.RatingValues = Item.RatingValues;
                        item.RatingCount = Item.RatingCount;

                        Item = mapper.Map<Item>(item);
                        Item.ImgUrl = uploadImages.UpdateImage(ImgUrl, item.File);

                        if (Item.RatingValues == null)
                        {
                            Item.RatingValues = 0;
                        }
                        if (Item.RatingCount == null)
                        {
                            Item.RatingCount = 0;
                        }

                        if (item.RaitingValue != 0)
                        {
                            Item.RatingCount++;
                            Item.RatingValues += item.RaitingValue;
                        }

                        _context.Update(Item);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ItemExists(item.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }

                    return RedirectToAction(nameof(Index));
                }
                ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name", item.CityId);
                ViewData["MemberId"] = new SelectList(supliers(), "Id", "FullName", item.MemberId);
                return View(item);
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
            
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.City)
                .Include(i => i.Member)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var item = await _context.Items.FindAsync(id);
                uploadImages.DeleteImage(item.ImgUrl);
                _context.Items.Remove(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
                
           
        }

        public IActionResult AddCategories(int id)
        {
            try
            {
                ViewBag.ItemId = id;
                return PartialView(_context.Categories.Include(ic => ic.ItemsCategories).ToList());
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
           
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCategories(int CategoryId, int ItemId)
        {
           try
            {
                Item item = _context.Items.Include(i => i.ItemsCategories).FirstOrDefault(i => i.Id == ItemId);
                if (item == null)
                {
                    return NotFound();
                }
                ItemCategory itemCategory = item.ItemsCategories.FirstOrDefault(ic => ic.CategoryId == CategoryId && ic.ItemId == ItemId);
                if (item.ItemsCategories == null)
                {
                    return NotFound();
                }

                if (itemCategory != null)
                {
                    item.ItemsCategories.Remove(itemCategory);
                    _context.SaveChanges();
                    return PartialView(await _context.Categories.Include(ic => ic.ItemsCategories).ToListAsync());
                }
                itemCategory = new ItemCategory()
                {
                    CategoryId = CategoryId
                    ,
                    ItemId = ItemId
                };

                item.ItemsCategories.Add(itemCategory);
                _context.SaveChanges();
                return PartialView(_context.Categories.Include(wc => wc.ItemsCategories).ToList());
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }

           
        }
        public IActionResult AddSupCategories(int id)
        {
            try
            {
                ViewBag.ItemId = id;
                return PartialView(_context.SubCategories.Include(ic => ic.ItemsSupCategories).ToList());
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
            
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSupCategories(int CategoryId, int ItemId)
        {
           try
            {
                Item item = _context.Items.Include(i => i.ItemsSupCategories).FirstOrDefault(i => i.Id == ItemId);
                if (item == null)
                {
                    return NotFound();
                }
                ItemSupCategory itemSupCategory = item.ItemsSupCategories.FirstOrDefault(ic => ic.SubCategoryId == CategoryId && ic.ItemId == ItemId);
                if (item.ItemsSupCategories == null)
                {
                    return NotFound();
                }

                if (itemSupCategory != null)
                {
                    item.ItemsSupCategories.Remove(itemSupCategory);
                    _context.SaveChanges();
                    return PartialView(await _context.SubCategories.Include(ic => ic.ItemsSupCategories).ToListAsync());
                }
                itemSupCategory = new ItemSupCategory()
                {
                    SubCategoryId = CategoryId
                    ,
                    ItemId = ItemId
                };

                item.ItemsSupCategories.Add(itemSupCategory);
                _context.SaveChanges();
                return PartialView(_context.SubCategories.Include(wc => wc.ItemsSupCategories).ToList());
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }

            
        }

        public async Task<IActionResult> SortByCategory(int id)
        {
            try
            {
                var ItemsByCategory = new List<Item>();
                if (id != 0)
                {
                    ItemsByCategory = await (from c in _context.Categories
                                             from i in _context.Items.Include(i => i.City).Include(i => i.Member)
                                             from ic in _context.ItemsCategories
                                             where c.Id == ic.CategoryId && i.Id == ic.ItemId && c.Id == id
                                             select i).ToListAsync();
                }
                else
                {
                    ItemsByCategory = await (_context.Items.Include(i => i.City).Include(i => i.Member)).ToListAsync();
                }

                return PartialView("Items", ItemsByCategory);
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
            
        }

        public async Task<IActionResult> SortBySupCategory(int id)
        {
            try
            {
                var ItemsBySupCategory = new List<Item>();
                if (id != 0)
                {
                    ItemsBySupCategory = await (from c in _context.SubCategories
                                                from i in _context.Items.Include(i => i.City).Include(i => i.Member)
                                                from ic in _context.ItemsSupCategories
                                                where c.Id == ic.SubCategoryId && i.Id == ic.ItemId && c.Id == id
                                                select i).ToListAsync();
                }
                else
                {
                    ItemsBySupCategory = await (_context.Items.Include(i => i.City).Include(i => i.Member)).ToListAsync();
                }

                return PartialView("Items", ItemsBySupCategory);
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
           
        }

        public List<Member> supliers()
        {
            var Suppliers = (from m in _context.Members.Include(m => m.City)
                                  from r in _context.Roless
                                  from mr in _context.MemberRoles
                                  where mr.MemberId == m.Id && mr.RoleId == r.Id && r.Name == "Supplier"
                                  select m).ToList();
            return Suppliers;
        }

        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.Id == id);
        }
    }
}
