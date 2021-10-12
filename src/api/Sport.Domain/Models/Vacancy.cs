using System;
using System.Collections.Generic;
using System.Text;

namespace Sport.Domain.Models
{
    public class Vacancy
    {
        public int Id { get; set; }

        public int ComplexId { get; set; }

        public int PositionId { get; set; }

        public Position Position { get; set; }

        public string Title { get; set; }

        public string Details { get; set; }

        public DateTime PostedDate { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
