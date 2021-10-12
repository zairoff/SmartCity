using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.API.DTOs.PocketDto
{
    public class PocketResponseDto
    {
        public int Id { get; set; }

        public string Pocket { get; set; }

        public decimal PricePerMonth { get; set; }
    }
}
