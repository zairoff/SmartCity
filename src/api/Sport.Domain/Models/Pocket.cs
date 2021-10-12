using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sport.Domain.Models
{
    public class Pocket
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal PricePerMonth { get; set; }
    }
}
