using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.API.DTOs.TrainerDto
{
    public class TrainerCreateDto
    {
        [Required]
        public int ComplexId { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public int SportTypeId { get; set; }
    }
}
