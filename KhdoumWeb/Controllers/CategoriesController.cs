using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KhdoumWeb.Data;
using KhdoumWeb.Models;
using KhdoumWeb.Helpers;
using AutoMapper;
using KhdoumWeb.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace KhdoumWeb.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UploadImages uploadImages;
        private readonly IMapper mapper;

        public CategoriesController(ApplicationDbContext context, UploadImages uploadImages, IMapper mapper)
        {
            _context = context;
            this.uploadImages = uploadImages;
            this.mapper = mapper;
            uploadImages.Folder = "categories";
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            try
            {
                return View(await _context.Categories.ToListAsync());

            }
            catch
            {
                return RedirectToAction(nameof(Index),"Home");
            }

        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var category = await _context.Categories
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (category == null)
                {
                    return NotFound();
                }

                return View(category);
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
            
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryViewModel category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Category Category = mapper.Map<Category>(category);
                    Category.ImgUrl = uploadImages.AddImage(category.File);

                    _context.Add(Category);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(category);
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
           
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var category = await _context.Categories.FindAsync(id);
                if (category == null)
                {
                    return NotFound();
                }
                var CategoryViewModel = mapper.Map<CategoryViewModel>(category);
                return View(CategoryViewModel);
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
            
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryViewModel category)
        {
          
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var Category = _context.Categories.AsNoTracking().FirstOrDefault(c => c.Id == id);
                    string ImgUrl = Category.ImgUrl;
                    Category = mapper.Map<Category>(category);

                    Category.ImgUrl = uploadImages.UpdateImage(ImgUrl, category.File);

                    _context.Update(Category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var category = await _context.Categories
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (category == null)
                {
                    return NotFound();
                }

                return View(category);
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
           
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);
                uploadImages.DeleteImage(category.ImgUrl);
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
            
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
