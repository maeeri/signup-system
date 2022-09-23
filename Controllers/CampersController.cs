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

            var camper = await _context.Camper
                .FirstOrDefaultAsync(m => m.Id == id);
            if (camper == null)
            {
                return NotFound();
            }

            return View(camper);
        }

        // GET: Campers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Camper == null)
            {
                return NotFound();
            }

            var camper = await _context.Camper.FindAsync(id);
            if (camper == null)
            {
                return NotFound();
            }
            return View(camper);
        }

        // POST: Campers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,StreetAddress,PostalCode,City,DoB,Guardian")] Camper camper)
        {
            if (id != camper.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(camper);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CamperExists(camper.Id))
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
            return View(camper);
        }

        // GET: Campers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Camper == null)
            {
                return NotFound();
            }

            var camper = await _context.Camper
                .FirstOrDefaultAsync(m => m.Id == id);
            if (camper == null)
            {
                return NotFound();
            }

            return View(camper);
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
    }
}
