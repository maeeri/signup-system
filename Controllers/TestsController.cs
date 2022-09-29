using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CampSignUpProject.Media;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SignUpProject.Data;
using SignUpProject.Models;

namespace SignUpProject.Controllers
{
    public class TestsController : Controller
    {
        private readonly SignUpProjectContext _context;

        public TestsController(SignUpProjectContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> PopulateDatabase()
        {
            await CreateCamps();
            await SaveRandomCounselors();
            await SaveRandomPeople();
            await MoveCampers();

            return RedirectToAction("Index", "Home");
        }

        public async Task CreateCamps()
        {
            var camps = new List<Camp>
            {
                new Camp()
                {
                    Capacity = 20,
                    End = new DateTime(2023, 6, 2),
                    Start = new DateTime(2023, 5, 30),
                    Location = "Leiripaikka",
                    Name = "Toukokuun leiri"
                },
                new Camp()
                {
                    Capacity = 35,
                    End = new DateTime(2023, 6, 20),
                    Start = new DateTime(2023, 6, 18),
                    Location = "Leiripaikka",
                    Name = "Kesäkuun leiri"
                },
                new Camp()
                {
                    Capacity = 15,
                    End = new DateTime(2023, 7, 15),
                    Start = new DateTime(2023, 7, 8),
                    Location = "Leiripaikka",
                    Name = "Heinäkuun leiri"
                },
                new Camp()
                {
                    Capacity = 80,
                    End = new DateTime(2023, 8, 30),
                    Start = new DateTime(2023, 8, 25),
                    Location = "Suurleiripaikka",
                    Name = "Elokuun suurleiri"
                }
            };
            await _context.Camp.AddRangeAsync(camps);
            await _context.SaveChangesAsync();
        }

        //for populating the database with test data/campers
        public async Task SaveRandomPeople()
        {
            var personsPath = "./DbData/fakepersons.json";
            var guardiansPath = "./DbData/fakeguardians.json";

            var jsonPersons = System.IO.File.ReadAllText(personsPath);
            var jsonGuardians = System.IO.File.ReadAllText(guardiansPath);

            var fakeCampers = JsonSerializer.Deserialize<FakePerson>(jsonPersons);
            var fakeCampersList = fakeCampers.data.ToList();

            var fakeGuardians = JsonSerializer.Deserialize<FakePerson>(jsonGuardians);
            var fakeGuardiansList = fakeGuardians.data.ToList();

            ViewModel viewModel = new ViewModel();
            viewModel.Campers = new List<Camper>();

            foreach (var person in fakeGuardiansList)
            {
                var guardian = new Guardian()
                {
                    Name = person.firstname + " " + person.lastname,
                    Email = person.email,
                    Tel = person.phone
                };

                _context.Add(guardian);
                _context.SaveChanges();
            }

            for (int i = 0; i < fakeCampersList.Count; i++)
            {

                viewModel.Guardians = await _context.Guardian.ToListAsync();

                var camper = new Camper()
                {
                    FirstName = fakeCampersList[i].firstname,
                    LastName = fakeCampersList[i].lastname,
                    StreetAddress = fakeCampersList[i].address.street,
                    DoB = Convert.ToDateTime(fakeCampersList[i].birthday),
                    PostalCode = fakeCampersList[i].address.zipcode,
                    City = fakeCampersList[i].address.city,
                    Guardian = viewModel.Guardians[(int)Math.Floor((double)i/2)].Id
                };
                _context.Camper.Add(camper);
                _context.SaveChanges();

                viewModel.Camper = _context.Camper.FirstOrDefault(x =>
                    x.FirstName == camper.FirstName && x.LastName == camper.LastName && x.DoB == camper.DoB);
                var allergies = new List<Allergy>();
                var rand = new Random();
                var max = rand.Next(0, 5);
                var oR = rand.Next(1, 3);
                string[] items = { "some", "other", "yet another", "one more", "and one more still", "the last one" };
                
                for (int j = 0; j < max; j++)
                {
                    allergies.Add(new Allergy()
                    {
                        Item = items[rand.Next(0, 5)],
                        Severity = (AllergySeverity)oR,
                        Camper = viewModel.Camper.Id
                    });
                }

                max = rand.Next(5);
                var medication = new List<Medication>();
                string[] medItems = { "med 1", "med 2", "med 3", "med 4", "med 5", "med 6" };
                string[] inst = { "take when needed", "needs help in taking", "independent" };
                for (int j = 0; j < max; j++)
                {
                    medication.Add(new Medication()
                    {
                        Item = medItems[rand.Next(0, 5)],
                        Instructions = inst[rand.Next(0, 2)],
                        Camper = viewModel.Camper.Id
                    });
                }

                viewModel.Camps = await _context.Camp.ToListAsync();

                viewModel.CampPeople = new CampPeople()
                {
                    Camper = viewModel.Camper.Id,
                    Camp = viewModel.Camps[rand.Next(0, 3)].Id,
                    RideIn = max % 2 == 0,
                    RideOut = max % 2 == 0
                };
                _context.AddRange(allergies);
                _context.AddRange(medication);
                _context.Add(viewModel.CampPeople);
                await _context.SaveChangesAsync();
            }
        }

        //for populating the database with test data/counselors
        public async Task SaveRandomCounselors()
        {
            var personsPath = "./DbData/fakecounselors.json";

            var jsonPersons = System.IO.File.ReadAllText(personsPath);

            var fakeCounselors = JsonSerializer.Deserialize<FakePerson>(jsonPersons);
            var fakeCounselorsList = fakeCounselors.data.ToList();

            ViewModel viewModel = new ViewModel();
            viewModel.Camps = await _context.Camp.ToListAsync();

            for (int i = 0; i < fakeCounselorsList.Count; i++)
            {
                var counselor = new Counselor()
                {
                    FirstName = fakeCounselorsList[i].firstname,
                    LastName = fakeCounselorsList[i].lastname,
                    StreetAddress = fakeCounselorsList[i].address.streetName,
                    City = fakeCounselorsList[i].address.city,
                    PostalCode = fakeCounselorsList[i].address.zipcode,
                    Email = fakeCounselorsList[i].email,
                    Tel = fakeCounselorsList[i].phone
                };

                _context.Add(counselor);
                _context.SaveChanges();
                viewModel.Counselor =
                    _context.Counselor.FirstOrDefault(x => x.Email == counselor.Email && x.Tel == counselor.Tel);

                viewModel.Staff = new Staff()
                {
                    Counselor = viewModel.Counselor.Id,
                };

                if (i < 4)
                    viewModel.Staff.Camp = viewModel.Camps[0].Id;
                else if (i < 8)
                    viewModel.Staff.Camp = viewModel.Camps[1].Id;
                else if (i < 10)
                    viewModel.Staff.Camp = viewModel.Camps[2].Id;
                else
                    viewModel.Staff.Camp = viewModel.Camps[3].Id;

                var anyoneInCharge = false;
                var campStaff = _context.Staff
                    .Where(x => x.Camp == viewModel.Staff.Camp).ToList();
                if (campStaff.Count > 0)
                    anyoneInCharge = campStaff.Any(x => x.IsInCharge);

                viewModel.Staff.IsInCharge = !anyoneInCharge;

                _context.Add(viewModel.Staff);
                _context.SaveChanges();

            }

        }

        //for testing purposes, to populate camps evenly
        public async Task MoveCampers()
        {
            for (int i = 0; i <= 3; i++)
            {
                ViewModel viewModel = new ViewModel();
                viewModel.Camps = await _context.Camp.ToListAsync();
                viewModel.AllCampPeople = _context.CampPeople.Where(x => x.Camp == viewModel.Camps[i].Id).ToList();
                viewModel.Campers = new List<Camper>();
                viewModel.Camp = await _context.Camp.FirstOrDefaultAsync(x => x.Id == viewModel.Camps[i].Id);

                foreach (var campPeople in viewModel.AllCampPeople)
                {
                    viewModel.Campers.Add(_context.Camper.FirstOrDefault(x => x.Id == campPeople.Camper));
                }

                while (viewModel.Camp.Capacity < viewModel.Campers.Count)
                {
                    viewModel.AllCampPeople = _context.CampPeople.Where(x => x.Camp == viewModel.Camp.Id).ToList();
                    viewModel.CampPeople = viewModel.AllCampPeople[^1];
                    if (i < 3)
                        viewModel.CampPeople.Camp = viewModel.Camps[i+1].Id;
                    viewModel.Camper = _context.Camper.FirstOrDefault(x => x.Id == viewModel.CampPeople.Camper);
                    viewModel.Campers.Remove(viewModel.Camper);
                    await _context.SaveChangesAsync();
                }

                _context.UpdateRange(viewModel.AllCampPeople);
                await _context.SaveChangesAsync();
            }
        }
    }
}
