using Sport.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.API.DTOs.SportEventDto
{
    public class SportEventResponseDto
    {
        public int Id { get; set; }

        public string EventName { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }
    }
}
