using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.API.DTOs.EventWinnerDto
{
    public class EventWinnerCreateDto
    {
        [Required]
        public int ParticipantId { get; set; }

        [Required]
        public int Place { get; set; }
    }
}
