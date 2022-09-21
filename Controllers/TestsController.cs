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

        public async Task PopulateDatabase()
        {
            await SaveRandomCounselors();
            await SaveRandomPeople();
            await MoveCampers();
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

            for (int i = 0; i < fakeCampersList.Count; i++)
            {
                var guardian = new Guardian()
                {
                    Name = fakeGuardiansList[i].firstname + fakeGuardiansList[i].lastname,
                    Email = fakeGuardiansList[i].email,
                    Tel = fakeGuardiansList[i].phone
                };

                _context.Add(guardian);
                _context.SaveChanges();
                viewModel.Guardian = _context.Guardian.FirstOrDefault(x => x.Email == guardian.Email && x.Name == guardian.Name);
                var camper = new Camper()
                {
                    FirstName = fakeCampersList[i].firstname,
                    LastName = fakeCampersList[i].lastname,
                    StreetAddress = fakeCampersList[i].address.street,
                    DoB = Convert.ToDateTime(fakeCampersList[i].birthday),
                    PostalCode = fakeCampersList[i].address.zipcode,
                    City = fakeCampersList[i].address.city,
                    Guardian = viewModel.Guardian.Id
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
                        Item = items[max],
                        Severity = (AllergySeverity)oR,
                        Camper = viewModel.Camper.Id
                    });
                }

                max = rand.Next(5);
                var medication = new List<Medication>();
                for (int j = 0; j < max; j++)
                {
                    medication.Add(new Medication()
                    {
                        Item = "medicine",
                        Instructions = "take when needed",
                        Camper = viewModel.Camper.Id
                    });
                }

                int campId = rand.Next(3, 7);
                viewModel.Camp = _context.Camp.FirstOrDefault(x => x.Id == campId);

                viewModel.CampPeople = new CampPeople()
                {
                    Camper = viewModel.Camper.Id,
                    Camp = viewModel.Camp.Id,
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
                    viewModel.Staff.Camp = 1;
                else if (i < 8)
                    viewModel.Staff.Camp = 2;
                else if (i < 13)
                    viewModel.Staff.Camp = 3;
                else
                    viewModel.Staff.Camp = 4;

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
            for (int i = 3; i <= 6; i++)
            {
                ViewModel viewModel = new ViewModel();
                viewModel.AllCampPeople = _context.CampPeople.Where(x => x.Camp == i).ToList();
                viewModel.Campers = new List<Camper>();
                viewModel.Camp = await _context.Camp.FirstOrDefaultAsync(x => x.Id == i);

                foreach (var campPeople in viewModel.AllCampPeople)
                {
                    viewModel.Campers.Add(_context.Camper.FirstOrDefault(x => x.Id == campPeople.Camper));
                }

                while (viewModel.Camp.Capacity < viewModel.Campers.Count)
                {
                    viewModel.AllCampPeople = _context.CampPeople.Where(x => x.Camp == i).ToList();
                    viewModel.CampPeople = viewModel.AllCampPeople[^1];
                    viewModel.CampPeople.Camp = 5;
                    viewModel.Camper = _context.Camper.FirstOrDefault(x => x.Id == viewModel.CampPeople.Camper);
                    viewModel.Campers.Remove(viewModel.Camper);
                    _context.SaveChanges();
                }

                _context.UpdateRange(viewModel.AllCampPeople);
                await _context.SaveChangesAsync();
            }
        }
    }
}
