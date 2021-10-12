using System;
using System.Collections.Generic;
using System.Text;

namespace Sport.Domain.Models
{
    public class Employee
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int ComplexId { get; set; }

        public string PersonId { get; set; }

        public int PositionId { get; set; }

        public Position Position { get; set; }
    }
}
