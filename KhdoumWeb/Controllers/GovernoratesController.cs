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
    public class GovernoratesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GovernoratesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Governorates
        public async Task<IActionResult> Index()
        {
            try
            {
                return View(await _context.Governorates.ToListAsync());

            }
            catch
            {
                return RedirectToAction(nameof(Index),"Home");
            }
        }

        // GET: Governorates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var governorate = await _context.Governorates
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (governorate == null)
                {
                    return NotFound();
                }

                return View(governorate);
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
           
        }

        // GET: Governorates/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Governorates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Governorate governorate)
        {
            if (ModelState.IsValid)
            {
                _context.Add(governorate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(governorate);
        }

        // GET: Governorates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var governorate = await _context.Governorates.FindAsync(id);
            if (governorate == null)
            {
                return NotFound();
            }
            return View(governorate);
        }

        // POST: Governorates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Governorate governorate)
        {
            if (id != governorate.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(governorate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GovernorateExists(governorate.Id))
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
            return View(governorate);
        }

        // GET: Governorates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var governorate = await _context.Governorates
                .FirstOrDefaultAsync(m => m.Id == id);
            if (governorate == null)
            {
                return NotFound();
            }

            return View(governorate);
        }

        // POST: Governorates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var governorate = await _context.Governorates.FindAsync(id);
            _context.Governorates.Remove(governorate);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GovernorateExists(int id)
        {
            return _context.Governorates.Any(e => e.Id == id);
        }
    }
}
