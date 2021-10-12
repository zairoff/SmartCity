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
    public class SportGroupService : ISportGroupService
    {
        private readonly IRepository<SportGroup> _groupRepository;

        public SportGroupService(IRepository<SportGroup> groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task<SportGroup> AddAsync(SportGroup group)
        {
            var existing = await _groupRepository.Get()
                                            .Where(t => t.Name == group.Name && t.SportTypeId == group.SportTypeId)
                                            .FirstOrDefaultAsync();

            if (existing != null)
                throw new ResourceExistException($"{group.Name} already exist");

            await _groupRepository.AddAsync(group);

            return group;
        }

        public async Task<SportGroup> DeleteAsync(int id)
        {
            var group = await _groupRepository.Get()
                                            .Where(s => s.Id == id)
                                            .Include(s => s.SportType)
                                            .FirstOrDefaultAsync();

            if (group == null)
                throw new NotFoundException("Not found");

            await _groupRepository.DeleteAsync(group);

            return group;
        }

        public async Task<IEnumerable<SportGroup>> GetAllAsync()
        {
            return await _groupRepository.GetAll().Include(s => s.SportType).ToListAsync();
        }

        public async Task<SportGroup> GetAsync(int id)
        {
            return await _groupRepository.Get().Where(t => t.Id == id)
                                        .Include(s => s.SportType)
                                        .FirstOrDefaultAsync();
        }

        public async Task<SportGroup> UpdateAsync(int id, SportGroup group)
        {
            var existing = await _groupRepository.Get()
                                                .Include(s => s.SportType)
                                                .Where(t => t.Id == id)
                                                .FirstOrDefaultAsync();

            if (existing == null)
                throw new NotFoundException("Not found");

            var duplicate = await _groupRepository.Get()
                                            .Where(t => t.Name == group.Name && t.SportTypeId == group.SportTypeId)
                                            .FirstOrDefaultAsync();

            if (duplicate != null)
                throw new ResourceExistException($"{group.Name} already exist");

            existing.Name = group.Name;

            await _groupRepository.UpdateAsync(existing);

            return existing;
        }
    }
}
