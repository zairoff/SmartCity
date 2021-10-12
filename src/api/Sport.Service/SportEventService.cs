using Microsoft.EntityFrameworkCore;
using Sport.Domain.Models;
using Sport.Infrastructure.Base;
using Sport.Service;
using Sport.Service.Exceptions;
using Sport.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.Service
{
    public class SportEventService : ISportEventService
    {
        private readonly IRepository<SportEvent> _sportEventRepository;

        public SportEventService(IRepository<SportEvent> sportEventRepository)
        {
            _sportEventRepository = sportEventRepository;
        }

        public async Task<IEnumerable<SportEvent>> GetAllAsync()
        {
            return await _sportEventRepository.GetAll().ToListAsync();
        }

        public async Task<SportEvent> GetAsync(int id)
        {
            return await _sportEventRepository.Get().Where(e => e.Id == id).FirstOrDefaultAsync();
        }

        public async Task<SportEvent> AddAsync(SportEvent sportEvent)
        {           
            var existing = await _sportEventRepository.Get().Where(e => e.Date == sportEvent.Date).FirstOrDefaultAsync();

            if (existing != null)
                throw new ResourceExistException("SportComplex has an event in this period");

            await _sportEventRepository.AddAsync(sportEvent);

            return sportEvent;
        }

        public async Task<SportEvent> UpdateAsync(int id, SportEvent sportEvent)
        {
            var exsiting = await _sportEventRepository.Get().Where(e => e.Id == id).FirstOrDefaultAsync();

            if (exsiting == null)
                throw new NotFoundException("Not found");

            exsiting.Name = sportEvent.Name;
            exsiting.Description = sportEvent.Description;
            exsiting.Date = sportEvent.Date;

            await _sportEventRepository.UpdateAsync(exsiting);

            return exsiting;
        }

        public async Task<SportEvent> DeleteAsync(int id)
        {
            var sportEvent = await _sportEventRepository.Get().Where(e => e.Id == id).FirstOrDefaultAsync();

            if (sportEvent == null)
                throw new NotFoundException("Not found");

            await _sportEventRepository.DeleteAsync(sportEvent);

            return sportEvent;
        }

        public async Task<IEnumerable<SportEvent>> GetByComplexIdAsync(int complexId)
        {
            return await _sportEventRepository.GetAll().Where(e => e.ComplexId == complexId).ToListAsync();
        }
    }
}
