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
    public class SubCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;
        private readonly UploadImages uploadImages;

        public SubCategoriesController(ApplicationDbContext context, IMapper mapper, UploadImages uploadImages)
        {
            _context = context;
            this.mapper = mapper;
            this.uploadImages = uploadImages;
            uploadImages.Folder = "SubCategory";
        }

        // GET: SubCategories
        public async Task<IActionResult> Index()
        {
            return View(await _context.SubCategories.ToListAsync());
        }

        // GET: SubCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subCategory = await _context.SubCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subCategory == null)
            {
                return NotFound();
            }

            return View(subCategory);
        }

        // GET: SubCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SubCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubCategoryViewModel subCategory)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var SubCategory = mapper.Map<SubCategory>(subCategory);
                    SubCategory.ImgUrl = uploadImages.AddImage(subCategory.File);

                    _context.Add(SubCategory);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(subCategory);

            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
           
        }

        // GET: SubCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subCategory = await _context.SubCategories.FindAsync(id);
            if (subCategory == null)
            {
                return NotFound();
            }
            var subCategoryViewModel = mapper.Map<SubCategoryViewModel>(subCategory);

            return View(subCategoryViewModel);
        }

        // POST: SubCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SubCategoryViewModel subCategory)
        {
            if (id != subCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try

                {
                    var SubCategory = _context.SubCategories.AsNoTracking().FirstOrDefault(c => c.Id == subCategory.Id);
                    string ImgUrl = SubCategory.ImgUrl;
                    SubCategory = mapper.Map<SubCategory>(subCategory);
                    SubCategory.ImgUrl = uploadImages.UpdateImage(ImgUrl, subCategory.File);
                    _context.Update(SubCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubCategoryExists(subCategory.Id))
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
            return View(subCategory);
        }

        // GET: SubCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subCategory = await _context.SubCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subCategory == null)
            {
                return NotFound();
            }

            return View(subCategory);
        }

        // POST: SubCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subCategory = await _context.SubCategories.FindAsync(id);
            uploadImages.DeleteImage(subCategory.ImgUrl);
            _context.SubCategories.Remove(subCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubCategoryExists(int id)
        {
            return _context.SubCategories.Any(e => e.Id == id);
        }
    }
}
