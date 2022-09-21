using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SignUpProject.Models
{
    public class CampPeople
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Camp")]
        public int Camp { get; set; }
        [Required]
        [ForeignKey("Camper")]
        public int Camper { get; set; }

        public bool RideIn { get; set; }
        public bool RideOut { get; set; }

    }

    public class Camper
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("First name")]
        public string? FirstName { get; set; }
        [DisplayName("Last name")]
        public string? LastName { get; set; }
        [DisplayName("Street address")]
        public string? StreetAddress { get; set; }
        public int PostalCode { get; set; }
        public string? City { get; set; }
        [DisplayFormat(DataFormatString = "{0: d.M.yyyy}")]
        public DateTime DoB { get; set; }

        [ForeignKey("Guardian")]
        public int Guardian { get; set; }

        public virtual Allergy[] Allergies { get; set; }
        public virtual Medication[] Medication { get; set; }
    }

    public class Allergy
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Camper")]
        public int Camper { get; set; }

        public string? Item { get; set; }
        public AllergySeverity Severity { get; set; }
    }

    public class Medication
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Camper")]
        public int Camper { get; set; }

        [DisplayName("Medication")]
        public string? Item { get; set; }
        [DisplayName("Instructions for use")]
        public string? Instructions { get; set; }
    }

    public class Guardian
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public int Tel { get; set; }
    }

    public enum AllergySeverity
    {
        High = 1,
        Medicated = 2,
        Mild = 3
    }
}
