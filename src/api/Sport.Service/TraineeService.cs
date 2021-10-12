using Microsoft.EntityFrameworkCore;
using Sport.Domain.Models;
using Sport.Infrastructure.Base;
using Sport.Service.Exceptions;
using Sport.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.Service
{
    public class TraineeService : ITraineeService
    {
        private readonly IRepository<Trainee> _traineeRepository;

        public TraineeService(IRepository<Trainee> traineeRepository)
        {
            _traineeRepository = traineeRepository;
        }

        public async Task<Trainee> AddAsync(Trainee trainee)
        {
            var existing = await _traineeRepository.Get()
                                                .Where(t => t.ComplexId == trainee.ComplexId &&
                                                t.PersonId == trainee.PersonId)
                                                .FirstOrDefaultAsync();

            if (existing != null)
                throw new ResourceExistException($"Trainee with PersonId:{trainee.PersonId} already exist");         

            await _traineeRepository.AddAsync(trainee);

            return trainee;
        }

        public async Task<Trainee> DeleteAsync(int id)
        {
            var trainee = await _traineeRepository.Get()
                                            .Where(t => t.Id == id)
                                            .Include(t => t.Group)
                                            .ThenInclude(t => t.SportType)
                                            .Include(t => t.Pocket)
                                            .FirstOrDefaultAsync();

            if (trainee == null)
                throw new NotFoundException("Not found");

            await _traineeRepository.DeleteAsync(trainee);

            return trainee;
        }

        public async Task<IEnumerable<Trainee>> GetAllAsync()
        {
            return await _traineeRepository.GetAll()                                      
                                        .Include(t => t.Group)
                                        .ThenInclude(t => t.SportType)
                                        .Include(t => t.Pocket)
                                        .ToListAsync();
        }

        public async Task<Trainee> GetAsync(int id)
        {
            return await _traineeRepository.Get()
                                        .Where(t => t.Id == id)                                        
                                        .Include(t => t.Group)
                                        .ThenInclude(t => t.SportType)
                                        .Include(t => t.Pocket)
                                        .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Trainee>> GetByPaymentStatusAsync(int complexId, bool isPaid)
        {
            return await _traineeRepository.GetAll()
                                        .Where(t => t.ComplexId == complexId && t.IsPaid == isPaid)                                        
                                        .Include(t => t.Group)
                                        .ThenInclude(t => t.SportType)
                                        .Include(t => t.Pocket)
                                        .ToListAsync();
        }

        public async Task<Trainee> GetByPersonIdAsync(int complexId, string personId)
        {
            return await _traineeRepository.Get()
                                        .Where(t => t.ComplexId == complexId && t.PersonId == personId)                                        
                                        .Include(t => t.Group)
                                        .ThenInclude(t => t.SportType)
                                        .Include(t => t.Pocket)
                                        .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Trainee>> GetByPocketIdAsync(int complexId, int pocketId)
        {
            return await _traineeRepository.GetAll()
                                        .Where(t => t.ComplexId == complexId && t.PocketId == pocketId)                                        
                                        .Include(t => t.Group)
                                        .ThenInclude(t => t.SportType)
                                        .Include(t => t.Pocket)
                                        .ToListAsync();
        }

        public async Task<IEnumerable<Trainee>> GetByGroupIdAsync(int complexId, int groupId)
        {
            return await _traineeRepository.GetAll()
                                        .Where(t => t.ComplexId == complexId && t.GroupId == groupId)                                        
                                        .Include(t => t.Group)
                                        .ThenInclude(t => t.SportType)
                                        .Include(t => t.Pocket)
                                        .ToListAsync();
        }

        public async Task<Trainee> UpdateAsync(int id, Trainee trainee)
        {
            var existing = await _traineeRepository.Get()
                                                .Where(t => t.Id == id)
                                                .FirstOrDefaultAsync();

            if (existing == null)
                throw new NotFoundException("Not found");

            existing.GroupId = trainee.GroupId;
            existing.PocketId = trainee.PocketId;
            existing.IsPaid = trainee.IsPaid;

            await _traineeRepository.UpdateAsync(existing);

            return existing; 
        }

        public async Task<IEnumerable<Trainee>> GetByComplexIdAsync(int complexId)
        {
            return await _traineeRepository.GetAll()
                                        .Where(t => t.ComplexId == complexId)
                                        .Include(t => t.Group)
                                        .ThenInclude(t => t.SportType)
                                        .Include(t => t.Pocket)
                                        .ToListAsync();
        }
    }
}
