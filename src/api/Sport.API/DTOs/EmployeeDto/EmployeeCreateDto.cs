using System.ComponentModel.DataAnnotations;

namespace Sport.API.DTOs.EmployeeDto
{
    public class EmployeeCreateDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public int ComplexId { get; set; }

        [Required]
        public string PersonId { get; set; }

        [Required]
        public int PositionId { get; set; }
    }
}
