using Sport.API.DTOs.EmployeeDto;
using Sport.API.DTOs.SportTypeDto;
using Sport.Domain.Models;

namespace Sport.API.DTOs.TrainerDto
{
    public class TrainerResponseDto
    {
        public int Id { get; set; }

        public EmployeeResponseDto Employee { get; set; }

        public SportTypeResponseDto SportType { get; set; }
    }
}
