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
    public class TrainerService : ITrainerService
    {
        private readonly IRepository<Trainer> _trainerRepository;

        public TrainerService(IRepository<Trainer> trainerRepository)
        {
            _trainerRepository = trainerRepository;
        }

        public async Task<Trainer> AddAsync(Trainer trainer) 
        {
            var employee = await _trainerRepository.Get()
                                            .Where(t => t.ComplexId == trainer.ComplexId &&
                                            t.EmployeeId == trainer.EmployeeId)
                                            .FirstOrDefaultAsync();

            if (employee != null)
                throw new ResourceExistException("Employee is already trainer");                                    

            await _trainerRepository.AddAsync(trainer);

            return trainer;
        }        

        public async Task<IEnumerable<Trainer>> GetAllAsync()
        {
            return await _trainerRepository.GetAll()
                                        .Include(e => e.SportType)
                                        .Include(e => e.Employee)
                                        .ThenInclude(emp => emp.Position)
                                        .ToListAsync();
        }

        public async Task<Trainer> GetAsync(int id)
        {
            return await _trainerRepository.Get()
                                        .Where(t => t.Id == id)
                                        .Include(e => e.SportType)
                                        .Include(e => e.Employee)
                                        .ThenInclude(emp => emp.Position)
                                        .FirstOrDefaultAsync();
        }

        public async Task<Trainer> GetByEmployeeIdAsync(int complexId, int employeeId)
        {
            return await _trainerRepository.Get()
                                        .Where(t => t.ComplexId == complexId &&
                                        t.EmployeeId == employeeId)
                                        .Include(e => e.SportType)
                                        .Include(e => e.Employee)
                                        .ThenInclude(emp => emp.Position)
                                        .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Trainer>> GetBySportTypeIdAsync(int complexId, int sportTypeId)
        {
            return await _trainerRepository.GetAll()
                                        .Where(t => t.ComplexId == complexId &&
                                        t.SportTypeId == sportTypeId)
                                        .Include(e => e.SportType)
                                        .Include(e => e.Employee)
                                        .ThenInclude(emp => emp.Position)
                                        .ToListAsync();
        }

        public async Task<Trainer> UpdateAsync(int id, Trainer trainer)
        {
            var existing = await _trainerRepository.Get()
                                                .Where(t => t.Id == id)
                                                .FirstOrDefaultAsync();

            if (existing == null)
                throw new NotFoundException("Not found");

            existing.SportTypeId = trainer.SportTypeId;

            await _trainerRepository.UpdateAsync(existing);

            return existing;
        }

        public async Task<Trainer> DeleteAsync(int id)
        {
            var trainer = await _trainerRepository.Get()
                                        .Where(t => t.Id == id)
                                        .Include(e => e.SportType)
                                        .Include(e => e.Employee)
                                        .ThenInclude(emp => emp.Position)
                                        .FirstOrDefaultAsync();

            if (trainer == null)
                throw new NotFoundException("Not found");

            await _trainerRepository.DeleteAsync(trainer);

            return trainer;
        }

        public async Task<IEnumerable<Trainer>> GetByComplexIdAsync(int complexId)
        {
            return await _trainerRepository.GetAll()
                                       .Where(t => t.ComplexId == complexId)
                                       .Include(e => e.SportType)
                                       .Include(e => e.Employee)
                                       .ThenInclude(emp => emp.Position)
                                       .ToListAsync();
        }
    }
}
