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

        // GET: Counselors
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> Counselors()
        {
            var viewModel = new ViewModel();
            viewModel.Counselors = await _context.Counselor.ToListAsync();
            viewModel.CompleteStaff = await _context.Staff.ToListAsync();
            viewModel.Camps = await _context.Camp.ToListAsync();

            return View(viewModel);
        }

        // GET: Counselors/Details/5
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Counselor == null)
            {
                return NotFound();
            }

            var counselor = await _context.Counselor
                .FirstOrDefaultAsync(m => m.Id == id);
            if (counselor == null)
            {
                return NotFound();
            }

            var viewModel = await GetCounselorViewModel(id);

            return View(viewModel);
        }

        // GET: Counselors/Create
        [Authorize(Roles = "Admin")]
        public IActionResult AddCounselor()
        {
            var viewModel = new ViewModel();
            if (_context.Camp != null) viewModel.Camps = _context.Camp.ToList();
            return View(viewModel);
        }

        // POST: Counselors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddCounselor([Bind("Staff,Counselor")] ViewModel viewModel)
        {
            try
            {
                _context.Counselor?.Add(viewModel.Counselor!);
                _context.SaveChanges();

                if (viewModel.Staff != null)
                {
                    viewModel.Staff.Counselor = viewModel.Counselor!.Id;
                    _context.Staff?.Add(viewModel.Staff);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                if (_context.Camp != null) viewModel.Camps = _context.Camp.ToList();
                return View(viewModel);
            }
        }

        // GET: Counselors/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Counselor == null)
            {
                return NotFound();
            }

            var counselor = await _context.Counselor.FindAsync(id);
            if (counselor == null)
            {
                return NotFound();
            }

            var viewModel = await GetCounselorViewModel(id);

            return View(viewModel);
        }

        // POST: Counselors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int id, [Bind("Counselor, CompleteStaff")] ViewModel viewModel)
        {
            if (id != viewModel.Counselor!.Id)
            {
                return NotFound();
            }

            if (viewModel.Counselor != null)
            {
                try
                {
                    _context.Update(viewModel.Counselor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CounselorExists(viewModel.Counselor.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Counselors));
            }
            return View(viewModel);
        }

        // GET: Counselors/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Counselor == null)
            {
                return NotFound();
            }

            var viewModel = await GetCounselorViewModel(id);
            
            if (viewModel.Counselor == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        // POST: Counselors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var viewModel = await GetCounselorViewModel(id);

            if (_context.Counselor == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Counselor'  is null.");
            }

            if (viewModel.Counselor != null)
            {
                _context.Counselor.Remove(viewModel.Counselor);
            }

            if (viewModel.CompleteStaff != null)
            {
                _context.Staff.RemoveRange(viewModel.CompleteStaff);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        private bool CounselorExists(int id)
        {
            return (_context.Counselor?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private async Task<ViewModel> GetCounselorViewModel(int? id)
        {
            var viewModel = new ViewModel();
            viewModel.Counselor = await _context.Counselor.FirstOrDefaultAsync(x => x.Id == id);
            viewModel.CompleteStaff = await _context.Staff.Where(x => x.Counselor == id).ToListAsync();
            viewModel.Camps = new List<Camp>();

            foreach (var staff in viewModel.CompleteStaff)
            {
                viewModel.Camps.Add((await _context.Camp.FirstOrDefaultAsync(x => x.Id == staff.Camp))!);
            }

            return viewModel;
        }
    }
}
