using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KhdoumWeb.Data;
using KhdoumWeb.Helpers;
using KhdoumWeb.Models.ViewModels;
using KhdoumWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace KhdoumWeb.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;
        private readonly UploadImages uploadImages;

        
        public CustomersController(ApplicationDbContext context, IMapper mapper, UploadImages uploadImages)
        {
            _context = context;
            this.mapper = mapper;
            this.uploadImages = uploadImages;
            uploadImages.Folder = "Members";
        }

        // GET: Members
        public async Task<IActionResult> Index()
        {
            var suppliers = await (from m in _context.Members.Include(m => m.City)
                                   from r in _context.Roless
                                   from mr in _context.MemberRoles
                                   where mr.MemberId == m.Id && mr.RoleId == r.Id && r.Name == "Customer"
                                   select m).ToListAsync();

            return View(suppliers);
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
                member.ItemsCount = 0;
                if (ModelState.IsValid)
                {
                    var Member = mapper.Map<Member>(member);
                    Member.ImgUrl = uploadImages.AddImage(member.File);
                    Member.ActivationCode = RandomURL.GetURL();
                    _context.Add(Member);
                    var created = await _context.SaveChangesAsync();

                    if (created >= 0)
                    {
                        //add Customer to role
                        var supplier = _context.Members.ToList().LastOrDefault();
                        var role = _context.Roless.FirstOrDefault(r => r.Name == "Customer");
                        if (role != null && supplier != null)
                        {
                            MemberRole SupplierRole = new MemberRole();
                            SupplierRole.MemberId = supplier.Id;
                            SupplierRole.RoleId = role.Id;
                            _context.MemberRoles.Add(SupplierRole);
                            _context.SaveChanges();
                        }
                    }






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
            member.ItemsCount = 0;
            var Member = _context.Members.AsNoTracking().FirstOrDefault(m => m.Id == member.Id);
            member.Password = Member.Password;
            member.ConfirmPassword = Member.Password;
            if (ModelState.IsValid)
            {
                try
                {
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


        //get all customers
        public List<Member> customers()
        {
            var Customers = (from m in _context.Members.Include(m => m.City)
                             from r in _context.Roless
                             from mr in _context.MemberRoles
                             where mr.MemberId == m.Id && mr.RoleId == r.Id && r.Name == "Customer"
                             select m).ToList();
            return Customers;
        }
        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.Id == id);
        }
    }
}
