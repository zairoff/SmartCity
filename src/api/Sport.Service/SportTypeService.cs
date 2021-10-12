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
    public class SportTypeService : ISportTypeService
    {
        private readonly IRepository<SportType> _sportTypeRepository;

        public SportTypeService(IRepository<SportType> sportTypeRepository)
        {
            _sportTypeRepository = sportTypeRepository;
        }

        public async Task<IEnumerable<SportType>> GetAllAsync()
        {
            return await _sportTypeRepository.GetAll()
                                        .ToListAsync();
        }

        public async Task<SportType> GetAsync(int id)
        {
            return await _sportTypeRepository.Get()
                                        .Where(s => s.Id == id)
                                        .FirstOrDefaultAsync();
        }

        public async Task<SportType> AddAsync(SportType sportType)
        {
            var exist = await _sportTypeRepository.Get()
                                                .Where(s => s.Name == sportType.Name)
                                                .FirstOrDefaultAsync();

            if (exist != null)
                throw new ResourceExistException($"SportType: {sportType.Name} already exist");

            await _sportTypeRepository.AddAsync(sportType);

            return sportType;
        }

        public async Task<SportType> UpdateAsync(int id, SportType sportType)
        {
            var existing = await _sportTypeRepository.Get()
                                                .Where(s => s.Id == id)
                                                .FirstOrDefaultAsync();

            if (existing == null)
                throw new NotFoundException("Not found");

            var duplicate = await _sportTypeRepository.Get()
                                                .Where(s => s.Name == sportType.Name)
                                                .FirstOrDefaultAsync();

            if (duplicate != null)
                throw new ResourceExistException($"SportType: {sportType.Name} already exist");

            existing.Name = sportType.Name;

            await _sportTypeRepository.UpdateAsync(existing);

            return existing;
        }

        public async Task<SportType> DeleteAsync(int id)
        {
            var exist = await _sportTypeRepository.Get()
                                                .Where(s => s.Id == id)
                                                .FirstOrDefaultAsync();

            if (exist == null)
                throw new NotFoundException("Not found");

            await _sportTypeRepository.DeleteAsync(exist);

            return exist;                       
        } 
    }
}
