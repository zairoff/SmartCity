using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.API.DTOs.TrainerGroupDto
{
    public class TrainerGroupCreateDto
    {
        [Required]
        public int TrainerId { get; set; }

        [Required]
        public int GroupId { get; set; }
    }
}
