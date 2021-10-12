using Sport.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sport.Service.Interfaces
{
    public interface ITrainerGroupService
    {
        Task<IEnumerable<TrainerGroup>> GetAllAsync();

        Task<TrainerGroup> GetAsync(int id);

        Task<IEnumerable<TrainerGroup>> GetByTrainerIdAsync(int trainerId);

        Task<IEnumerable<TrainerGroup>> GetByGroupIdAsync(int groupId);

        Task<TrainerGroup> AddAsync(TrainerGroup trainerGroup);

        Task<TrainerGroup> DeleteAsync(int id);
    }
}
