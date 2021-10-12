using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sport.Domain.Models
{
    public class EventParticipant
    {
        public int Id { get; set; }

        public int SportEventId { get; set; }

        public SportEvent SportEvent { get; set; }

        public int TraineeId { get; set; }

        public Trainee Trainee { get; set; }
    }
}
