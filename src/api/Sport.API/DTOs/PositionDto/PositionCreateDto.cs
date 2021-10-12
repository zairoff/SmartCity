using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.API.DTOs.PositionDto
{
    public class PositionCreateDto
    {
        [Required]
        public string Position { get; set; }
    }
}
