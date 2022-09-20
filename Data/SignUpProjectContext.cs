using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignUpProject.Models;

namespace SignUpProject.Data
{
    public class SignUpProjectContext : DbContext
    {
        public SignUpProjectContext (DbContextOptions<SignUpProjectContext> options)
            : base(options)
        {
        }

        public DbSet<SignUpProject.Models.CampPeople> CampPeople { get; set; } = default!;

        public DbSet<SignUpProject.Models.Allergy> Allergy { get; set; }

        public DbSet<SignUpProject.Models.Camp> Camp { get; set; }

        public DbSet<SignUpProject.Models.Camper> Camper { get; set; }

        public DbSet<SignUpProject.Models.Counselor> Counselor { get; set; }

        public DbSet<SignUpProject.Models.Guardian> Guardian { get; set; }

        public DbSet<SignUpProject.Models.Medication> Medication { get; set; }

        public DbSet<SignUpProject.Models.Staff> Staff { get; set; }
    }
}
