using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.API.DTOs.VacancyDto
{
    public class VacancyCreateDto
    {
        [Required]
        public int ComplexId { get; set; }

        [Required]
        public int PositionId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Details { get; set; }

        [Required]
        public DateTime PostedDate { get; set; }
    }
}
