using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SignUpProject.Data;
using SignUpProject.Models;

namespace SignUpProject.Controllers
{
    public class CampersController : Controller
    {
        private readonly SignUpProjectContext _context;

        public CampersController(SignUpProjectContext context)
        {
            _context = context;
        }

        // GET: Campers
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> Index()
        {
              return View(await _context.Camper.ToListAsync());
        }

        // GET: Campers/Details/5
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Camper == null)
            {
                return NotFound();
            }

            var viewModel = await GetCamperViewModel(id);
            
            if (viewModel.Camper == null)
            {
                return NotFound();
            }
            
            return View(viewModel);
        }

        // GET: Campers/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            
            if (id == null || _context.Camper == null)
            {
                return NotFound();
            }

            var viewModel = await GetCamperViewModel(id);

            if (viewModel.Camper == null)
            {
                return NotFound();
            }
            

            return View(viewModel);
        }

        // POST: Campers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Camper, Allergies, CampPeople, Guardian")] ViewModel viewModel)
        {
            if (id != viewModel.Camper.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(viewModel.Camper);
                    _context.Update(viewModel.Guardian);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CamperExists(viewModel.Camper.Id))
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
            return View(viewModel);
        }

        // GET: Campers/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Camper == null)
            {
                return NotFound();
            }

            var viewModel = await GetCamperViewModel(id);
            
            if (viewModel.Camper == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        // POST: Campers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var viewModel = await GetCamperViewModel(id);
            if (_context.Camper == null)
            {
                return Problem("Entity set 'SignUpProjectContext.Camper'  is null.");
            }
            
            if (viewModel.Camper != null)
                _context.Camper.Remove(viewModel.Camper);

            if (viewModel.AllCampPeople.Count > 0)
                _context.CampPeople.RemoveRange(viewModel.AllCampPeople);

            if (viewModel.Allergies.Count > 0)
                _context.Allergy.RemoveRange(viewModel.Allergies);

            if (viewModel.Medications.Count > 0)
                _context.Medication.RemoveRange(viewModel.Medications);

            _context.SaveChanges(); //needs to be done here to check whether another camper has the same guardian and if not, delete guardian info

            viewModel.Campers = await _context.Camper.ToListAsync();
            var guardianIds = viewModel.Campers.Select(x => x.Guardian).ToList();

            if (!guardianIds.Contains(viewModel.Guardian.Id))
                _context.Remove(viewModel.Guardian);

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        private bool CamperExists(int id)
        {
            return _context.Camper.Any(e => e.Id == id);
        }

        public async Task<ViewModel> GetCamperViewModel(int? id)
        {
            var viewModel = new ViewModel();
            viewModel.Camper = await _context.Camper.FindAsync(id);
            viewModel.Camps = new List<Camp>();
            viewModel.AllCampPeople = await _context.CampPeople.Where(x => x.Camper == id).ToListAsync();
            viewModel.Allergies = await _context.Allergy.Where(x => x.Camper == id).ToListAsync();
            viewModel.Medications = await _context.Medication.Where(x => x.Camper == id).ToListAsync();
            viewModel.Guardian = await _context.Guardian.FirstOrDefaultAsync(x => x.Id == viewModel.Camper!.Guardian);

            foreach (var link in viewModel.AllCampPeople)
            {
                viewModel.Camps.Add((await _context.Camp.FirstOrDefaultAsync(x => x.Id == link.Camp))!);
            }

            return viewModel;
        }
    }
}
