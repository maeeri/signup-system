using SignUpProject.Data;
using SignUpProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SignUpProject.Controllers
{
    public class SignUpsController : Controller
    {
        private readonly SignUpProjectContext _context;

        public SignUpsController(SignUpProjectContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> SignUp()
        {
            ViewModel viewModel = new ViewModel();
            if (_context.Camp != null) viewModel.Camps = await _context.Camp.ToListAsync();
            return View(viewModel);
        }

        // GET: SignUpsController/Details/5
        public ActionResult Details(ViewModel viewModel)
        {
            return View();
        }

        // GET: SignUpsController/Create
        //public async Task<ActionResult> Create()
        //{
        //    var viewModel = new ViewModel();
        //    return View(viewModel);
        //}

        //save the signup info to database
        // POST: SignUpsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SignUp([Bind("Camp,Camper,Guardian,Allergies,Medications,CampPeople")] ViewModel viewModel)
        {
            int? id = null;
            try
            {
                viewModel.Camp = _context.Camp?.FirstOrDefault(x => x.Id == viewModel.Camp!.Id)!;

                var findGuardian = _context.Guardian?.FirstOrDefault(x => x.Email == viewModel.Guardian!.Email);

                if (findGuardian != null)
                {
                    //viewModel.Guardian.Id = findGuardian.Id;
                    //findGuardian = viewModel.Guardian;
                    viewModel.Guardian = findGuardian;
                }
                else
                {
                    await _context.Guardian!.AddAsync(viewModel.Guardian!);
                    await _context.SaveChangesAsync(); //has to happen here to get the id of the guardian for Camper
                }
                viewModel.Camper!.Guardian = viewModel.Guardian!.Id;

                var findCamper = _context.Camper!.FirstOrDefault(x =>
                    x.FirstName == viewModel.Camper.FirstName && 
                    x.LastName == viewModel.Camper.LastName &&
                    x.DoB == viewModel.Camper.DoB);

                if (findCamper != null)
                {
                    findCamper.StreetAddress = viewModel.Camper.StreetAddress;
                    findCamper.PostalCode = viewModel.Camper.PostalCode;
                    findCamper.City = viewModel.Camper.City;
                    viewModel.Camper = findCamper;
                }
                else
                {
                    await _context.Camper!.AddAsync(viewModel.Camper)!;
                    await _context.SaveChangesAsync(); //has to happen here to get the id of the camper for camppeople table
                }

                viewModel.CampPeople!.Camper = viewModel.Camper.Id;
                id = viewModel.Camper.Id;
                viewModel.CampPeople.Camp = viewModel.Camp.Id;
                await _context.CampPeople!.AddAsync(viewModel.CampPeople);
            

                if (viewModel.Allergies!.Count != 0)
                {
                    List<Allergy> tempAllergies = new List<Allergy>(); //needs to be done this way at this time, because empty input in form
                    foreach (var allergy in viewModel.Allergies)
                    {
                        if (allergy.Item != null)
                        {
                            allergy.Camper = viewModel.Camper.Id;
                            tempAllergies.Add(allergy);
                        }
                    }
                    viewModel.Allergies.Clear();
                    viewModel.Allergies.AddRange(tempAllergies);
                    await _context.Allergy?.AddRangeAsync(viewModel.Allergies)!;
                }
                if (viewModel.Medications!.Count != 0)
                {
                    List<Medication> tempMedicine = new List<Medication>(); //needs to be done this way at this time, because empty input in form
                    foreach (var medicine in viewModel.Medications)
                    {
                        if (medicine.Item != null)
                        {
                            medicine.Camper = viewModel.Camper.Id;
                            tempMedicine.Add(medicine);
                        }
                    }
                    viewModel.Medications.Clear();
                    viewModel.Medications.AddRange(tempMedicine);
                    await _context.Medication?.AddRangeAsync(viewModel.Medications)!;
                }

                await _context.SaveChangesAsync();
                //return View(viewModel);
                return RedirectToAction("Confirmation", "SignUps", new {@id=id});
            }
            catch
            {
                return RedirectToAction(nameof(SignUp));
            }
        }


        //shows signup details after signing up
        public async Task<ActionResult> Confirmation(int? id)
        {
            var viewModel = new ViewModel();
            viewModel.Camper = _context.Camper?.FirstOrDefault(x => x.Id == id)!;
            viewModel.AllCampPeople = _context.CampPeople?.Where(x => x.Camper == id).ToList()!;
            viewModel.Camps = new List<Camp>();
            viewModel.Guardian = _context.Guardian?.FirstOrDefault(x => x.Id == viewModel.Camper.Guardian)!;

            foreach (var campPeople in viewModel.AllCampPeople)
            {
                viewModel.Camps.Add(_context.Camp?.FirstOrDefault(x => x.Id == campPeople.Camp)!);
            }

            viewModel.Allergies = await _context.Allergy?.Where(x => x.Camper == id).ToListAsync()!;
            viewModel.Medications = await _context.Medication?.Where(x => x.Camper == id).ToListAsync()!;

            return View(viewModel);
        }

        // GET: SignUpsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SignUpsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // POST: SignUpsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCamper(int id)
        {
            ViewModel viewModel = new ViewModel();
            viewModel.Camper = _context.Camper?.FirstOrDefault(x => x.Id == id)!;
            viewModel.AllCampPeople = _context.CampPeople?.Where(x => x.Camper == id).ToList()!;
            viewModel.Allergies = _context.Allergy?.Where(x => x.Camper == id).ToList()!;
            viewModel.Medications = _context.Medication?.Where(x => x.Camper == id).ToList()!;
            _context.RemoveRange(viewModel);

            return RedirectToAction("Index", "Home");
        }

        // POST: SignUpsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
