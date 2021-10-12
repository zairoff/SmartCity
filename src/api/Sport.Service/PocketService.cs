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
    public class PocketService : IPocketService
    {
        private readonly IRepository<Pocket> _pocketRepository;

        public PocketService(IRepository<Pocket> pocketRepository) 
        {
            _pocketRepository = pocketRepository;
        }

        public async Task<Pocket> AddAsync(Pocket pocket)
        {
            var existing = await _pocketRepository.Get()
                                            .Where(p => p.Name == pocket.Name)
                                            .FirstOrDefaultAsync();

            if (existing != null)
                throw new ResourceExistException($"Pocket: {pocket.Name} already exist");

            await _pocketRepository.AddAsync(pocket);

            return pocket;
        }

        public async Task<Pocket> DeleteAsync(int id)
        {
            var pocket = await _pocketRepository.Get()
                                            .Where(p => p.Id == id)
                                            .FirstOrDefaultAsync();

            if (pocket == null)
                throw new NotFoundException("Not found");

            await _pocketRepository.DeleteAsync(pocket);

            return pocket;
        }

        public async Task<IEnumerable<Pocket>> GetAllAsync()
        {
            return await _pocketRepository.GetAll().ToListAsync();
        }

        public async Task<Pocket> GetAsync(int id)
        {
            return await _pocketRepository.Get()
                                        .Where(p => p.Id == id)
                                        .FirstOrDefaultAsync();
        }

        public async Task<Pocket> UpdateAsync(int id, Pocket pocket)
        {
            var existing = await _pocketRepository.Get()
                                        .Where(p => p.Id == id)
                                        .FirstOrDefaultAsync();

            if (existing == null)
                throw new NotFoundException("Not found");

            existing.PricePerMonth = pocket.PricePerMonth;

            await _pocketRepository.UpdateAsync(existing);

            return existing;
        }
    }
}
