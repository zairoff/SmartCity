using System;
using System.Collections.Generic;
using System.Text;

namespace Sport.Domain.Models
{
    public class Trainee
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int ComplexId { get; set; }

        public string PersonId { get; set; }

        public int GroupId { get; set; }

        public SportGroup Group { get; set; }

        public int PocketId { get; set; }

        public Pocket Pocket { get; set; }

        public bool IsPaid { get; set; }
    }
}
