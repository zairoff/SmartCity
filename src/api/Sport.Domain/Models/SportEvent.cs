using System;
using System.Collections.Generic;
using System.Text;

namespace Sport.Domain.Models
{
    public class SportEvent
    {
        public int Id { get; set; }

        public int ComplexId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }
    }
}
