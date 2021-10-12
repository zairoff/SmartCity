using System;
using System.Collections.Generic;
using System.Text;

namespace Sport.Domain.Models
{
    public class Applicant
    {
        public int Id { get; set; }

        public int VacancyId { get; set; }

        public Vacancy Vacancy { get; set; }

        public string PersonId { get; set; }
    }
}
