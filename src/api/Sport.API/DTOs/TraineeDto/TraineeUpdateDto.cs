using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.API.DTOs.TraineeDto
{
    public class TraineeUpdateDto
    {
        [Required]
        public int GroupId { get; set; }

        [Required]
        public int PocketId { get; set; }

        [Required]
        public bool IsPaid { get; set; }
    }
}
