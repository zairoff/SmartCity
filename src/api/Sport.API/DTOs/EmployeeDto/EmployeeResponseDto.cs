
using Sport.API.DTOs.PositionDto;

namespace Sport.API.DTOs.EmployeeDto
{
    public class EmployeeResponseDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int ComplexId { get; set; }

        public string PersonId { get; set; }

        public PositionResponseDto Position { get; set; }
    }
}
