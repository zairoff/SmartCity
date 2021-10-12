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
    public class VacancyService : IVacancyService
    {
        private readonly IRepository<Vacancy> _vacancyRepository;

        public VacancyService(IRepository<Vacancy> vacancyRepository)
        {
            _vacancyRepository = vacancyRepository;
        }

        public async Task<Vacancy> AddAsync(Vacancy vacancy)
        {
            await _vacancyRepository.AddAsync(vacancy);

            return vacancy;
        }

        public async Task<Vacancy> DeleteAsync(int id)
        {
            var vacancy = await _vacancyRepository.Get()
                                            .Where(v => v.Id == id)
                                            .Include(v => v.Position)
                                            .FirstOrDefaultAsync();

            if (vacancy == null)
                throw new NotFoundException("Not found");

            await _vacancyRepository.DeleteAsync(vacancy);

            return vacancy;
        }

        public async Task<IEnumerable<Vacancy>> GetAllAsync()
        {
            return await _vacancyRepository.GetAll()
                                        .Include(v => v.Position)
                                        .ToListAsync();
        }

        public async Task<Vacancy> GetAsync(int id)
        {
            return await _vacancyRepository.Get()
                                        .Where(v => v.Id == id)
                                        .Include(v => v.Position)
                                        .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Vacancy>> GetByComplexIdAsync(int complexId)
        {
            return await _vacancyRepository.GetAll()
                                        .Where(v => v.ComplexId == complexId)
                                        .Include(v => v.Position)
                                        .ToListAsync();
        }

        public async Task<IEnumerable<Vacancy>> GetByPositionIdAsync(int complexId, int positionId)
        {
            return await _vacancyRepository.GetAll()
                                        .Where(v => v.ComplexId == complexId &&
                                        v.PositionId == positionId)
                                        .Include(v => v.Position)
                                        .ToListAsync();
        }

        public async Task<IEnumerable<Vacancy>> GetByStatusAsync(int complexId, bool status)
        {
            return await _vacancyRepository.GetAll()
                                        .Where(v => v.ComplexId == complexId &&
                                        v.IsActive == status)
                                        .Include(v => v.Position)
                                        .ToListAsync();
        }

        public async Task<Vacancy> UpdateAsync(int id, Vacancy vacancy)
        {
            var existing = await _vacancyRepository.Get()
                                            .Where(v => v.Id == id)                                            
                                            .FirstOrDefaultAsync();

            if (existing == null)
                throw new NotFoundException("Not found");

            existing.PositionId = vacancy.PositionId;
            existing.Details = vacancy.Details;
            existing.Title = vacancy.Title;
            existing.IsActive = vacancy.IsActive;

            await _vacancyRepository.UpdateAsync(existing);

            return existing;
        }
    }
}
