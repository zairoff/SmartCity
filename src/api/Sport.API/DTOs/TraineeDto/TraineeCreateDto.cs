using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.API.DTOs.TraineeDto
{
    public class TraineeCreateDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public int ComplexId { get; set; }

        [Required]
        public string PersonId { get; set; }

        [Required]
        public int GroupId { get; set; }

        [Required]
        public int PocketId { get; set; }

        [Required]
        public bool IsPaid { get; set; }
    }
}
