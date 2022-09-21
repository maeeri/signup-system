using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SignUpProject.Models;
using SignUpProject.Data;

namespace SignUpProject.Controllers
{
    public class CounselorsController : Controller
    {
        private readonly SignUpProjectContext _context;

        public CounselorsController(SignUpProjectContext context)
        {
            _context = context;
        }

        // GET: Councelors
        public async Task<IActionResult> Index()
        {
            return _context.Counselor != null ?
                        View(await _context.Counselor.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Councelor'  is null.");
        }

        // GET: Councelors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Counselor == null)
            {
                return NotFound();
            }

            var councelor = await _context.Counselor
                .FirstOrDefaultAsync(m => m.Id == id);
            if (councelor == null)
            {
                return NotFound();
            }

            return View(councelor);
        }

        // GET: Councelors/Create
        public IActionResult AddCounselor()
        {
            var viewModel = new ViewModel();
            if (_context.Camp != null) viewModel.Camps = _context.Camp.ToList();
            return View(viewModel);
        }

        // POST: Councelors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddCounselor(/*[Bind("CompleteStaff,Counselor,Camps")]*/ ViewModel viewModel)
        {
            try
            {
                _context.Counselor!.Add(viewModel.Counselor!);
                var tempStaff = new List<Staff>();

                foreach (var staff in viewModel.CompleteStaff!)
                {
                    if (staff != null)
                        tempStaff.Add(staff);
                }

                _context.Staff!.AddRange(tempStaff);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                if (_context.Camp != null) viewModel.Camps = _context.Camp.ToList();
                return View(viewModel);
            }
        }

        // GET: Councelors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Counselor == null)
            {
                return NotFound();
            }

            var councelor = await _context.Counselor.FindAsync(id);
            if (councelor == null)
            {
                return NotFound();
            }
            return View(councelor);
        }

        // POST: Councelors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,Tel,StreetAddress,PostalCode,City")] Counselor councelor)
        {
            if (id != councelor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(councelor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CouncelorExists(councelor.Id))
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
            return View(councelor);
        }

        // GET: Councelors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Counselor == null)
            {
                return NotFound();
            }

            var councelor = await _context.Counselor
                .FirstOrDefaultAsync(m => m.Id == id);
            if (councelor == null)
            {
                return NotFound();
            }

            return View(councelor);
        }

        // POST: Councelors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Counselor == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Councelor'  is null.");
            }
            var councelor = await _context.Counselor.FindAsync(id);
            if (councelor != null)
            {
                _context.Counselor.Remove(councelor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CouncelorExists(int id)
        {
            return (_context.Counselor?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
