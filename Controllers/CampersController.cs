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
        public async Task<IActionResult> Index()
        {
              return View(await _context.Camper.ToListAsync());
        }

        // GET: Campers/Details/5
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
        public async Task<IActionResult> Edit(int id, [Bind("Camper, Allergies, CampPeople")] ViewModel viewModel)
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Camper == null)
            {
                return Problem("Entity set 'SignUpProjectContext.Camper'  is null.");
            }
            var camper = await _context.Camper.FindAsync(id);
            if (camper != null)
            {
                _context.Camper.Remove(camper);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CamperExists(int id)
        {
            return _context.Camper.Any(e => e.Id == id);
        }

        private async Task<ViewModel> GetCamperViewModel(int? id)
        {
            var viewModel = new ViewModel();
            viewModel.Camps = new List<Camp>();
            viewModel.AllCampPeople = await _context.CampPeople.Where(x => x.Camper == id).ToListAsync();
            viewModel.Allergies = await _context.Allergy.Where(x => x.Camper == id).ToListAsync();
            viewModel.Medications = await _context.Medication.Where(x => x.Camper == id).ToListAsync();
            viewModel.Guardian = await _context.Guardian.FirstOrDefaultAsync(x => x.Id == viewModel.Camper.Guardian);

            foreach (var link in viewModel.AllCampPeople)
            {
                viewModel.Camps.Add(await _context.Camp.FirstOrDefaultAsync(x => x.Id == link.Camp));
            }

            return viewModel;
        }
    }
}
