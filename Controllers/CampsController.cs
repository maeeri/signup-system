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

namespace SignUpProject.Controllers
{
    public class CampsController : Controller
    {
        private readonly SignUpProjectContext _context;

        public CampsController(SignUpProjectContext context)
        {
            _context = context;
        }


        // GET: Camps
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> Index()
        {
            var viewModel = new ViewModel();
            viewModel.Camps = await _context.Camp?.ToListAsync()!;
            viewModel.Campers = await _context.Camper.ToListAsync();
            viewModel.AllCampPeople = await _context.CampPeople.ToListAsync();
            viewModel.CompleteStaff = await _context.Staff.ToListAsync();
            viewModel.Counselors = await _context.Counselor.ToListAsync();

            return View(viewModel);
        }

        
        // GET: Camps/Details/5
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> Details(int? id)
        {
            var viewModel = await GetCampViewModel(id);

            if (id == null || _context.Camp == null)
            {
                return NotFound();
            }

            if (viewModel.Camp == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        // GET: Camps/Create
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
                _context.CampPeople.RemoveRange(_context.CampPeople.Where(x => x.Camp == id));
                _context.Staff.RemoveRange(_context.Staff.Where(x => x.Camp == id));
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CampExists(int id)
        {
            return (_context.Camp?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private async Task<ViewModel> GetCampViewModel(int? id)
        {
            var viewModel = new ViewModel();
            viewModel.Camp = await _context.Camp.FindAsync(id);
            viewModel.Counselors = new List<Counselor>();
            viewModel.CompleteStaff = await _context.Staff?.Where(x => x.Camp == id).ToListAsync()!;
            viewModel.AllCampPeople = await _context.CampPeople.Where(x => x.Camp == id).ToListAsync();
            viewModel.Campers = new List<Camper>();
            viewModel.Allergies = new List<Allergy>();

            foreach (var conn in viewModel.AllCampPeople)
            {
                var camper = await _context.Camper.FindAsync(conn.Camper);
                if (camper != null)
                    viewModel.Campers.Add(camper);
            }

            foreach (var staff in viewModel.CompleteStaff)
            {
                var counselor = await _context.Counselor.FindAsync(staff.Counselor);
                if (counselor != null)
                    viewModel.Counselors.Add(counselor);
            }

            //foreach (var camper in viewModel.Campers)
            //{
            //    var allergy = await _context.Allergy.FirstOrDefaultAsync(x => x.Camper == camper.Id);
            //    if (allergy != null)
            //    {
            //        viewModel.Allergies.Add(allergy);
            //    }
            //}

            viewModel.Allergies = viewModel.Allergies.OrderBy(x => x.Item).ToList();
            viewModel.Campers = viewModel.Campers.OrderBy(x => x.LastName).ToList();
            viewModel.Guardians = await _context.Guardian.ToListAsync();

            return viewModel;
        }

    }
}
