using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.API.DTOs.SportTypeDto
{
    public class SportTypeCreateDto
    {
        [Required]
        public string Sport { get; set; }
    }
}
