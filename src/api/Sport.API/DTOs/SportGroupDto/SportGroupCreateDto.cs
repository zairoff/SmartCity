using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.API.DTOs.SportGroupDto
{
    public class SportGroupCreateDto
    {
        [Required]
        public int SportTypeId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
