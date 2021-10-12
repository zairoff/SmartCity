using Sport.API.DTOs.SportGroupDto;
using Sport.API.DTOs.TrainerDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.API.DTOs.TrainerGroupDto
{
    public class TrainerGroupResponseDto
    {
        public int Id { get; set; }

        public TrainerResponseDto Trainer { get; set; }

        public SportGroupResponseDto Group { get; set; }
    }
}
