using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sport.API.DTOs.SportEventDto
{
    public class SportEventUpdateDto
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
