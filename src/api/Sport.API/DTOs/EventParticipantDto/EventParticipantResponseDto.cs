using Sport.API.DTOs.SportEventDto;
using Sport.API.DTOs.TraineeDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.API.DTOs.EventParticipantDto
{
    public class EventParticipantResponseDto
    {
        public int Id { get; set; }

        public SportEventResponseDto SportEvent { get; set; }

        public TraineeResponseDto Trainee { get; set; }
    }
}
