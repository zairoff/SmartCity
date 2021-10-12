using System.ComponentModel.DataAnnotations;

namespace Sport.API.DTOs.EmployeeDto
{
    public class EmployeeUpdateDto
    {
        [Required]
        public string PositionId { get; set; }
    }
}
