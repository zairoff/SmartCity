using System.ComponentModel.DataAnnotations;

namespace Sport.API.DTOs.PocketDto
{
    public class PocketUpdateDto
    {
        [Required]
        public decimal PricePerMonth { get; set; }
    }
}
