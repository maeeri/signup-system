using System.ComponentModel.DataAnnotations.Schema;

namespace SignUpProject.Models
{
    //viewmodel to view different models in same view
    [NotMapped]
    public class ViewModel
    {
        public List<Camper>? Campers { get; set; }
        public Camper? Camper { get; set; }

        public List<Counselor>? Counselors { get; set; }
        public Counselor? Counselor{ get; set; }

        public List<Staff>? CompleteStaff { get; set; }
        public Staff? Staff { get; set; }

        public List<Allergy>? Allergies { get; set; }
        public List<Medication>? Medications { get; set; }

        public List<Camp>? Camps { get; set; }
        public Camp? Camp{ get; set; }

        public List<CampPeople>? AllCampPeople { get; set; }
        public CampPeople? CampPeople { get; set; }

        public Guardian? Guardian { get; set; }
        public List<Guardian>? Guardians { get; set; }

        public UserProfile? UserProfile { get; set; }
    }
}
