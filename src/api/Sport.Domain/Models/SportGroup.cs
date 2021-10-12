    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sport.Domain.Models
{
    public class SportGroup
    {
        public int Id { get; set; }

        public int SportTypeId { get; set; }

        public SportType SportType { get; set; }

        public string Name { get; set; }
    }
}
