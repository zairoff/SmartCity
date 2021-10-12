using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sport.Domain.Models
{
    public class TrainerGroup
    {
        public int Id { get; set; }

        public int TrainerId { get; set; }

        public Trainer Trainer { get; set; }

        public int GroupId { get; set; }

        public SportGroup Group { get; set; }
    }
}
