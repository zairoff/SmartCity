using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.API.DTOs.PocketDto
{
    public class PocketCreateDto
    {
        [Required]
        public string Pocket { get; set; }

        [Required]
        public decimal PricePerMonth { get; set; }
    }
}
