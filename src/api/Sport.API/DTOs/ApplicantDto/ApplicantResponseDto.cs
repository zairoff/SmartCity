using Sport.API.DTOs.VacancyDto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.API.DTOs.ApplicantDto
{
    public class ApplicantResponseDto
    {
        public int Id { get; set; }

        public string PersonId { get; set; }

        public VacancyResponseDto Vacancy { get; set; }
    }
}
