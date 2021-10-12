using Sport.API.DTOs.SportTypeDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.API.DTOs.SportGroupDto
{
    public class SportGroupResponseDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public SportTypeResponseDto SportType { get; set; }
    }
}
