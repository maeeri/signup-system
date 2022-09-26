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
    public class DataController : Controller
    {
        private readonly SignUpProjectContext _context;

        public DataController(SignUpProjectContext context)
        {
            _context = context;
        }

        private bool AllergyExists(int id)
        {
            return _context.Allergy.Any(e => e.Id == id);
        }

        private bool CamperExists(int id)
        {
            return _context.Camper.Any(e => e.Id == id);
        }

        private bool CampPeopleExists(int id)
        {
            return _context.CampPeople.Any(e => e.Id == id);
        }

        private bool CampExists(int id)
        {
            return (_context.Camp?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private bool CouncelorExists(int id)
        {
            return (_context.Counselor?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private bool GuardianExists(int id)
        {
            return _context.Guardian.Any(e => e.Id == id);
        }

        private bool MedicationExists(int id)
        {
            return _context.Medication.Any(e => e.Id == id);
        }

        private bool StaffExists(int id)
        {
            return _context.Staff.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Search()
        {
            var viewModel = new ViewModel();
            viewModel.Campers = await _context.Camper.ToListAsync();
            viewModel.Counselors = await _context.Counselor.ToListAsync();
            viewModel.Camps = await _context.Camp.ToListAsync();

            return View(viewModel);
        }
    }
}
