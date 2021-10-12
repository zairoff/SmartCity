using Sport.API.DTOs.PositionDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.API.DTOs.VacancyDto
{
    public class VacancyResponseDto
    {
        public int Id { get; set; }

        public int ComplexId { get; set; }

        public PositionResponseDto Position { get; set; }

        public string Title { get; set; }

        public string Details { get; set; }

        public DateTime PostedDate { get; set; }

        public bool IsActive { get; set; }
    }
}
