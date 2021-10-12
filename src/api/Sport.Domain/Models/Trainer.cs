
namespace Sport.Domain.Models
{
    public class Trainer
    {
        public int Id { get; set; }

        public int ComplexId { get; set; }

        public int EmployeeId { get; set; }

        public Employee Employee { get; set; }

        public int SportTypeId { get; set; }

        public SportType SportType { get; set; }        
    }
}
