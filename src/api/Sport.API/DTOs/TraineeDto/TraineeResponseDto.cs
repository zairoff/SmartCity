using Sport.API.DTOs.PocketDto;
using Sport.API.DTOs.SportGroupDto;
using Sport.API.DTOs.SportTypeDto;

namespace Sport.API.DTOs.TraineeDto
{
    public class TraineeResponseDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int ComplexId { get; set; }

        public string PersonId { get; set; }

        public bool IsPaid { get; set; }

        public SportGroupResponseDto Group { get; set; }

        public PocketResponseDto Pocket { get; set; }

    }
}
