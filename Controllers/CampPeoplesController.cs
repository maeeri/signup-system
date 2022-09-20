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
    public class CampPeoplesController : Controller
    {
        private readonly SignUpProjectContext _context;

        public CampPeoplesController(SignUpProjectContext context)
        {
            _context = context;
        }

        // GET: CampPeoples
        public async Task<IActionResult> Index()
        {
              return View(await _context.CampPeople.ToListAsync());
        }

        // GET: CampPeoples/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CampPeople == null)
            {
                return NotFound();
            }

            var campPeople = await _context.CampPeople
                .FirstOrDefaultAsync(m => m.Id == id);
            if (campPeople == null)
            {
                return NotFound();
            }

            return View(campPeople);
        }

        // GET: CampPeoples/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CampPeoples/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Camp,Camper,RideIn,RideOut")] CampPeople campPeople)
        {
            if (ModelState.IsValid)
            {
                _context.Add(campPeople);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(campPeople);
        }

        // GET: CampPeoples/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CampPeople == null)
            {
                return NotFound();
            }

            var campPeople = await _context.CampPeople.FindAsync(id);
            if (campPeople == null)
            {
                return NotFound();
            }
            return View(campPeople);
        }

        // POST: CampPeoples/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Camp,Camper,RideIn,RideOut")] CampPeople campPeople)
        {
            if (id != campPeople.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(campPeople);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CampPeopleExists(campPeople.Id))
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
            return View(campPeople);
        }

        // GET: CampPeoples/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CampPeople == null)
            {
                return NotFound();
            }

            var campPeople = await _context.CampPeople
                .FirstOrDefaultAsync(m => m.Id == id);
            if (campPeople == null)
            {
                return NotFound();
            }

            return View(campPeople);
        }

        // POST: CampPeoples/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CampPeople == null)
            {
                return Problem("Entity set 'SignUpProjectContext.CampPeople'  is null.");
            }
            var campPeople = await _context.CampPeople.FindAsync(id);
            if (campPeople != null)
            {
                _context.CampPeople.Remove(campPeople);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CampPeopleExists(int id)
        {
          return _context.CampPeople.Any(e => e.Id == id);
        }
    }
}
