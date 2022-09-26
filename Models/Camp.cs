using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SignUpProject.Models
{
    public class Camp
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Camp name is required")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Camp location is required")]
        public string? Location { get; set; }
        [Required(ErrorMessage = "Camp capacity is required")]
        public int Capacity { get; set; }
        [Required(ErrorMessage = "Start date is required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0: d.M.yyyy}")]
        public DateTime Start { get; set; }
        [Required(ErrorMessage = "End date is required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0: d.M.yyyy}")]
        public DateTime End { get; set; }

        
        public override string ToString()
        {
            string res = $"{Id};{Name};{Location};{Capacity};{Start};{End}";
            return res;
        }
    }
}
