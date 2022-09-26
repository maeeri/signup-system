using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SignUpProject.Models
{
    public class Staff
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Camp")]
        public int Camp { get; set; }

        [ForeignKey("Counselor")]
        public int Counselor { get; set; }

        public bool IsInCharge { get; set; }
    }

    public class Counselor
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "First name is required")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        public string? LastName { get; set; }
        [Required(ErrorMessage = "Email address is required")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Phone number is required")]
        [DataType(DataType.PhoneNumber)]
        public string? Tel { get; set; }
        [Required(ErrorMessage = "Street address is required")]
        public string? StreetAddress { get; set; }
        [Required(ErrorMessage = "Postal code is required")]
        public string? PostalCode { get; set; }
        [Required(ErrorMessage = "City is required")]
        public string? City { get; set; }
    }
}
