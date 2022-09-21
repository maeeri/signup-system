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
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string? Tel { get; set; }
        public string? StreetAddress { get; set; }
        public string? PostalCode { get; set; }
        public string? City { get; set; }
    }
}
