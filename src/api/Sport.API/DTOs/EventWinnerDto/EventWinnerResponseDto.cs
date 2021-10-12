using Sport.API.DTOs.EventParticipantDto;
using Sport.API.DTOs.SportEventDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.API.DTOs.EventWinnerDto
{
    public class EventWinnerResponseDto
    {
        public int Id { get; set; }        

        public int Place { get; set; }

        public EventParticipantResponseDto Participant { get; set; }
    }
}
