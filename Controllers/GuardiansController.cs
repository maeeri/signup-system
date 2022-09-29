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
    public class GuardiansController : Controller
    {
        private readonly SignUpProjectContext _context;

        public GuardiansController(SignUpProjectContext context)
        {
            _context = context;
        }

        // GET: Guardians
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
              return View(await _context.Guardian.ToListAsync());
        }

        // GET: Guardians/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Guardian == null)
            {
                return NotFound();
            }

            var guardian = await _context.Guardian
                .FirstOrDefaultAsync(m => m.Id == id);
            if (guardian == null)
            {
                return NotFound();
            }

            return View(guardian);
        }

        // GET: Guardians/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Guardians/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Tel")] Guardian guardian)
        {
            if (ModelState.IsValid)
            {
                _context.Add(guardian);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(guardian);
        }

        // GET: Guardians/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Guardian == null)
            {
                return NotFound();
            }

            var guardian = await _context.Guardian.FindAsync(id);
            if (guardian == null)
            {
                return NotFound();
            }
            return View(guardian);
        }

        // POST: Guardians/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Tel")] Guardian guardian)
        {
            if (id != guardian.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(guardian);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GuardianExists(guardian.Id))
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
            return View(guardian);
        }

        // GET: Guardians/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Guardian == null)
            {
                return NotFound();
            }

            var guardian = await _context.Guardian
                .FirstOrDefaultAsync(m => m.Id == id);
            if (guardian == null)
            {
                return NotFound();
            }

            return View(guardian);
        }

        // POST: Guardians/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Guardian == null)
            {
                return Problem("Entity set 'SignUpProjectContext.Guardian'  is null.");
            }
            var guardian = await _context.Guardian.FindAsync(id);
            if (guardian != null)
            {
                _context.Guardian.Remove(guardian);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GuardianExists(int id)
        {
            return _context.Guardian.Any(e => e.Id == id);
        }
    }
}
