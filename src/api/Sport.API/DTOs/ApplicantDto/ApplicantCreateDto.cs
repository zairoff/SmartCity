using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.API.DTOs.ApplicantDto
{
    public class ApplicantCreateDto
    {
        [Required]
        public int VacancyId { get; set; }

        [Required]
        public string PersonId { get; set; }
    }
}
