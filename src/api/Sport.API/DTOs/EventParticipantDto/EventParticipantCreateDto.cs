using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.API.DTOs.EventParticipantDto
{
    public class EventParticipantCreateDto
    {
        [Required]
        public int SportEventId { get; set; }

        [Required]
        public int TraineeId { get; set; }
    }
}
