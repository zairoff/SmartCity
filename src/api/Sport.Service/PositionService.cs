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
    public class PositionService : IPositionService
    {
        private readonly IRepository<Position> _positionRepository;

        public PositionService(IRepository<Position> positionRepository) 
        {
            _positionRepository = positionRepository;
        }

        public async Task<IEnumerable<Position>> GetAllAsync()
        {
            return await _positionRepository.GetAll().ToListAsync();
        }

        public async Task<Position> GetAsync(int id)
        {
            return await _positionRepository.Get().Where(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Position> AddAsync(Position position)
        {
            var existing = await _positionRepository.Get()
                                                .Where(p => p.Name == position.Name)
                                                .FirstOrDefaultAsync();

            if (existing != null)
                throw new ResourceExistException($"Positon {position.Name} already exist");

            await _positionRepository.AddAsync(position);

            return position;
        }

        public async Task<Position> UpdateAsync(int id, Position position)
        {
            var existing = await _positionRepository.Get().Where(p => p.Id == id).FirstOrDefaultAsync();

            if (existing == null)
                throw new NotFoundException("Not found");

            var duplicate = await _positionRepository.Get().Where(p => p.Name == position.Name)
                                                            .FirstOrDefaultAsync();

            if (duplicate != null)
                throw new ResourceExistException($"Positon {position.Name} already exist");

            existing.Name = position.Name;

            await _positionRepository.UpdateAsync(existing);

            return existing;
        }

        public async Task<Position> DeleteAsync(int id)
        {
            var position = await _positionRepository.Get().Where(p => p.Id == id).FirstOrDefaultAsync();

            if (position == null)
                throw new NotFoundException("Not found");

            await _positionRepository.DeleteAsync(position);

            return position;
        }
    }
}
