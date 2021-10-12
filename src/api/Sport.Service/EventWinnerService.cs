using Microsoft.EntityFrameworkCore;
using Sport.Domain.Models;
using Sport.Infrastructure.Base;
using Sport.Service.Exceptions;
using Sport.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sport.Service
{
    public class EventWinnerService : IEventWinnerService
    {
        private readonly IRepository<EventWinner> _repository;

        public EventWinnerService(IRepository<EventWinner> repository)
        {
            _repository = repository;
        }

        public async Task<EventWinner> AddAsync(EventWinner eventWinner)
        {
            await _repository.AddAsync(eventWinner);

            return eventWinner;
        }

        public async Task<EventWinner> DeleteAsync(int id)
        {
            var winner = await _repository.Get().Where(e => e.Id == id).FirstOrDefaultAsync();

            if (winner == null)
                throw new NotFoundException("Not Found");

            await _repository.DeleteAsync(winner);

            return winner;
        }

        public async Task<IEnumerable<EventWinner>> GetAllAsync()
        {
            return await _repository.GetAll().Include(e => e.Participant)
                                            .ThenInclude(t => t.Trainee)
                                            .Include(e => e.Participant)
                                            .ThenInclude(e => e.Trainee.Group)
                                            .Include(e => e.Participant)
                                            .ThenInclude(e => e.Trainee.Group.SportType)
                                            .Include(e => e.Participant)
                                            .ThenInclude(e => e.SportEvent)
                                            .ToListAsync();

        }

        public async Task<EventWinner> GetAsync(int id)
        {
            return await _repository.Get().Where(e => e.Id == id)                                                
                                                .Include(e => e.Participant)
                                                .ThenInclude(t => t.Trainee)
                                                .Include(e => e.Participant)
                                                .ThenInclude(e => e.Trainee.Group)
                                                .Include(e => e.Participant)
                                                .ThenInclude(e => e.Trainee.Group.SportType)
                                                .Include(e => e.Participant)
                                                .ThenInclude(e => e.SportEvent)
                                                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<EventWinner>> GetByEventIdAsync(int eventId)
        {
            return await _repository.GetAll().Include(e => e.Participant)
                                            .ThenInclude(t => t.Trainee)
                                            .Include(e => e.Participant)
                                            .ThenInclude(e => e.Trainee.Group)
                                            .Include(e => e.Participant)
                                            .ThenInclude(e => e.Trainee.Group.SportType)
                                            .Include(e => e.Participant)
                                            .ThenInclude(e => e.SportEvent)
                                            .Where(e => e.Participant.SportEventId == eventId)
                                            .ToListAsync();
        }

        public async Task<EventWinner> GetByParticipantIdAsync(int participanId)
        {
            return await _repository.Get().Where(e => e.ParticipantId == participanId)
                                                .Include(e => e.Participant)
                                                .ThenInclude(t => t.Trainee)
                                                .Include(e => e.Participant)
                                                .ThenInclude(e => e.Trainee.Group)
                                                .Include(e => e.Participant)
                                                .ThenInclude(e => e.Trainee.Group.SportType)
                                                .Include(e => e.Participant)
                                                .ThenInclude(e => e.SportEvent)
                                                .FirstOrDefaultAsync();
        }
    }
}
