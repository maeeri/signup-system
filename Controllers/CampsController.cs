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

        //[Authorize]
        // GET: Camps
        public async Task<IActionResult> Index()
        {
            var viewModel = new ViewModel();
            viewModel.Camps = await _context.Camp?.ToListAsync()!;

            return View(viewModel);
        }

        //[Authorize]
        // GET: Camps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var viewModel = new ViewModel();

            if (id == null || _context.Camp == null)
            {
                return NotFound();
            }

            viewModel.Camp = await _context.Camp
                .FirstOrDefaultAsync(m => m.Id == id);
            if (viewModel.Camp == null)
            {
                return NotFound();
            }

            viewModel.Counselors = new List<Counselor>();
            viewModel.CompleteStaff = await _context.Staff?.Where(x => x.Camp == id).ToListAsync()!;
            viewModel.AllCampPeople = await _context.CampPeople.Where(x => x.Camp == id).ToListAsync();
            viewModel.Campers = new List<Camper>();
            viewModel.Allergies = new List<Allergy>();

            foreach (var campPeople in viewModel.AllCampPeople)
            {
                viewModel.Campers.Add(_context.Camper.FirstOrDefault(x => x.Id == campPeople.Camper)!);
            }

            foreach (var camper in viewModel.Campers)
            {
                viewModel.Allergies.AddRange(await _context.Allergy.Where(x => x.Camper == camper.Id).ToListAsync());
            }

            viewModel.Allergies = viewModel.Allergies.OrderBy(x => x.Item).ToList();
            viewModel.Guardians = await _context.Guardian.ToListAsync();

            if (viewModel.CompleteStaff != null)
                foreach (var staff in viewModel.CompleteStaff)
                {
                    viewModel.Counselors.Add(
                        (await _context.Counselor?.FirstOrDefaultAsync(x => x.Id == staff.Counselor)!)!);
                }

            return View(viewModel);
        }

        // GET: Camps/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Camps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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
