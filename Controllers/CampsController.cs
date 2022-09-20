using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using SignUpProject.Data;
using SignUpProject.Models;

namespace CampSignUpProject.Controllers
{
    public class CampsController : Controller
    {
        private readonly SignUpProjectContext _context;

        public CampsController(SignUpProjectContext context)
        {
            _context = context;
        }

        //[Authorize]
        // GET: Camps
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> Index()
        {
            var viewModel = new ViewModel();
            viewModel.Camps = await _context.Camp?.ToListAsync()!;
            viewModel.AllCampPeople = await _context.CampPeople.ToListAsync();
            viewModel.CompleteStaff = await _context.Staff.ToListAsync();

            return View(viewModel);
        }

        //[Authorize]
        // GET: Camps/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            ViewModel viewModel = new ViewModel();
            viewModel.Counselors = new List<Counselor>();
            viewModel.Campers = new List<Camper>();
            viewModel.Camp = _context.Camp!.FirstOrDefault(x => x.Id == id);
            viewModel.CompleteStaff = _context.Staff!.Where(x => x.Camp == id).ToList();
            foreach (var staff in viewModel.CompleteStaff)
            {
                viewModel.Counselors!.Add(_context.Counselor!.FirstOrDefault(x => x.Id == staff.Counselor)!);
            }

            viewModel.AllCampPeople = _context.CampPeople.Where(x => x.Camp == id).ToList();

            foreach (var person in viewModel.AllCampPeople)
            {
                viewModel.Campers!.Add(_context.Camper.FirstOrDefault(x => x.Id == person.Camper)!);
            }

            if (id == null || _context.Camp == null)
            {
                return NotFound();
            }

            var camp = await _context.Camp
                .FirstOrDefaultAsync(m => m.Id == id);
            if (camp == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        // GET: Camps/Create
        [Authorize(Roles="Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Camps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,Location,Capacity,Start,End")] Camp camp)
        {
            if (ModelState.IsValid)
            {
                _context.Add(camp);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(camp);
        }

        // GET: Camps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Camp == null)
            {
                return NotFound();
            }

            var camp = await _context.Camp.FindAsync(id);
            if (camp == null)
            {
                return NotFound();
            }
            return View(camp);
        }

        // POST: Camps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Location,Capacity,Start,End")] Camp camp)
        {
            if (id != camp.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(camp);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CampExists(camp.Id))
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
            return View(camp);
        }

        // GET: Camps/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Camp == null)
            {
                return NotFound();
            }

            var camp = await _context.Camp
                .FirstOrDefaultAsync(m => m.Id == id);
            if (camp == null)
            {
                return NotFound();
            }

            return View(camp);
        }

        // POST: Camps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Camp == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Camp'  is null.");
            }
            var camp = await _context.Camp.FindAsync(id);
            if (camp != null)
            {
                _context.Camp.Remove(camp);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CampExists(int id)
        {
            return (_context.Camp?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
