using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KhdoumWeb.Data;
using KhdoumWeb.Models;
using Microsoft.AspNetCore.Authorization;

namespace KhdoumWeb.Controllers
{
    [Authorize]
    public class CitiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cities
        public async Task<IActionResult> Index()
        {
            try
            {
                var applicationDbContext = _context.Cities.Include(c => c.Governorate);
                return View(await applicationDbContext.ToListAsync());
            }
            catch
            {
                return RedirectToAction(nameof(Index),"Home");
            }
            
        }

        // GET: Cities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var city = await _context.Cities
                    .Include(c => c.Governorate)
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (city == null)
                {
                    return NotFound();
                }

                return View(city);
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
          
        }

        // GET: Cities/Create
        public IActionResult Create()
        {
            try
            {
                ViewData["GovernorateId"] = new SelectList(_context.Governorates, "Id", "Name");
                return View();
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
            
        }

        // POST: Cities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,GovernorateId")] City city)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(city);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["GovernorateId"] = new SelectList(_context.Governorates, "Id", "Name", city.GovernorateId);
                return View(city);
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
           
        }

        // GET: Cities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var city = await _context.Cities.FindAsync(id);
                if (city == null)
                {
                    return NotFound();
                }
                ViewData["GovernorateId"] = new SelectList(_context.Governorates, "Id", "Name", city.GovernorateId);
                return View(city);
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
           
        }

        // POST: Cities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,GovernorateId")] City city)
        {
            
            if (id != city.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(city);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CityExists(city.Id))
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
            ViewData["GovernorateId"] = new SelectList(_context.Governorates, "Id", "Name", city.GovernorateId);
            return View(city);
        }

        // GET: Cities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var city = await _context.Cities
                    .Include(c => c.Governorate)
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (city == null)
                {
                    return NotFound();
                }

                return View(city);
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
          
        }

        // POST: Cities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var city = await _context.Cities.FindAsync(id);
                _context.Cities.Remove(city);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
            
        }

        private bool CityExists(int id)
        {
            return _context.Cities.Any(e => e.Id == id);
        }
    }
}
