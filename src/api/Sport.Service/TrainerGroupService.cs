using Microsoft.EntityFrameworkCore;
using Sport.Domain.Models;
using Sport.Infrastructure.Base;
using Sport.Service.Exceptions;
using Sport.Service.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.Service
{
    public class TrainerGroupService : ITrainerGroupService
    {
        private readonly IRepository<TrainerGroup> _repository;

        public TrainerGroupService(IRepository<TrainerGroup> repository)
        {
            _repository = repository;
        }

        public async Task<TrainerGroup> AddAsync(TrainerGroup trainerGroup)
        {
            var exist = await _repository.Get().Where(t => t.GroupId == trainerGroup.GroupId &&
                                                        t.TrainerId == trainerGroup.TrainerId)
                                                .FirstOrDefaultAsync();

            if (exist != null)
                throw new ResourceExistException("Trainer has already enrolled to this group");

            await _repository.AddAsync(trainerGroup);

            return trainerGroup;
        }

        public async Task<TrainerGroup> DeleteAsync(int id)
        {
            var exist = await _repository.Get().Where(t => t.Id == id)
                                                 .Include(t => t.Trainer)
                                                 .Include(t => t.Group)
                                                 .FirstOrDefaultAsync();

            if (exist == null)
                throw new NotFoundException("Not Found");

            await _repository.DeleteAsync(exist);

            return exist;
        }

        public async Task<IEnumerable<TrainerGroup>> GetAllAsync()
        {
            return await _repository.GetAll()
                                    .Include(t => t.Trainer)
                                    .ThenInclude(t => t.Employee)
                                    .Include(t => t.Trainer)
                                    .ThenInclude(t => t.Employee.Position)
                                    .Include(t => t.Group)
                                    .ThenInclude(t => t.SportType)
                                    .ToListAsync();
        }

        public async Task<TrainerGroup> GetAsync(int id)
        {
            return await _repository.Get()
                                    .Where(t => t.Id == id)
                                    .Include(t => t.Trainer)
                                    .Include(t => t.Group)
                                    .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TrainerGroup>> GetByGroupIdAsync(int groupId)
        {
            return await _repository.GetAll()
                                    .Where(t => t.GroupId == groupId)
                                    .Include(t => t.Trainer)
                                    .Include(t => t.Group)
                                    .ToListAsync();
        }

        public async Task<IEnumerable<TrainerGroup>> GetByTrainerIdAsync(int trainerId)
        {
            return await _repository.GetAll()
                                    .Where(t => t.TrainerId == trainerId)
                                    .Include(t => t.Trainer)
                                    .Include(t => t.Group)
                                    .ToListAsync();
        }
    }
}
