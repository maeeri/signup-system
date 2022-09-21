﻿using System.ComponentModel.DataAnnotations;

namespace SignUpProject.Models
{
    public class Camp
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Location { get; set; }
        public int Capacity { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0: d.M.yyyy}")]
        public DateTime Start { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0: d.M.yyyy}")]
        public DateTime End { get; set; }
    }
}