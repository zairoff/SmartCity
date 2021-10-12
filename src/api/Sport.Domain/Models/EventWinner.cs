using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sport.Domain.Models
{
    public class EventWinner
    {
        public int Id { get; set; }

        public int ParticipantId { get; set; }

        public EventParticipant Participant { get; set; }

        public int Place { get; set; }
    }
}
