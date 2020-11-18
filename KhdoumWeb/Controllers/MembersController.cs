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
    public class MembersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;
        private readonly UploadImages uploadImages;

        public MembersController(ApplicationDbContext context, IMapper mapper, UploadImages uploadImages)
        {
            _context = context;
            this.mapper = mapper;
            this.uploadImages = uploadImages;
            uploadImages.Folder = "Members";
        }

        // GET: Members
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Members.Include(m => m.City);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .Include(m => m.City)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // GET: Members/Create
        public IActionResult Create()
        {
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name");
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MemberViewModel member)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var Member = mapper.Map<Member>(member);
                    Member.ImgUrl = uploadImages.AddImage(member.File);
                    Member.ActivationCode = RandomURL.GetURL();
                    _context.Add(Member);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name", member.CityId);
                return View(member);
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
           
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            var Member = mapper.Map<MemberViewModel>(member);

            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name", member.CityId);
            return View(Member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MemberViewModel member)
        {
            if (id != member.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var Member = _context.Members.AsNoTracking().FirstOrDefault(m => m.Id == member.Id);
                    member.ActivationCode = Member.ActivationCode;
                    string ImgUrl = Member.ImgUrl;

                    Member = mapper.Map<Member>(member);
                    Member.ImgUrl = uploadImages.UpdateImage(ImgUrl, member.File);

                    _context.Update(Member);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.Id))
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
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name", member.CityId);
            return View(member);
        }

        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .Include(m => m.City)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = await _context.Members.FindAsync(id);
            uploadImages.DeleteImage(member.ImgUrl);
            _context.Members.Remove(member);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.Id == id);
        }
    }
}
