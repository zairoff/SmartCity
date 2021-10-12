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
    public class EventParticipantService : IEventParticipantService
    {
        private readonly IRepository<EventParticipant> _repository;

        public EventParticipantService(IRepository<EventParticipant> repository)
        {
            _repository = repository;
        }

        public async Task<EventParticipant> AddAsync(EventParticipant eventParticipant)
        {
            var participant = await _repository.Get().Where(e => e.SportEventId == eventParticipant.SportEventId
                                                && e.TraineeId == eventParticipant.TraineeId)
                                                .FirstOrDefaultAsync();

            if (participant != null)
                throw new ResourceExistException("Trainee already submitted to event");

            await _repository.AddAsync(eventParticipant);

            return eventParticipant;
        }

        public async Task<EventParticipant> DeleteAsync(int id)
        {
            var participant = await _repository.Get().Where(e => e.Id == id).FirstOrDefaultAsync();

            if (participant == null)
                throw new NotFoundException("Not Found");

            await _repository.DeleteAsync(participant);

            return participant;
        }

        public async Task<IEnumerable<EventParticipant>> GetAllAsync()
        {
            return await _repository.GetAll()
                                    .Include(e => e.SportEvent)
                                    .Include(e => e.Trainee)
                                    .ThenInclude(e => e.Group)
                                    .Include(e => e.Trainee)
                                    .ThenInclude(e => e.Group.SportType)
                                    .ToListAsync();
        }

        public async Task<EventParticipant> GetAsync(int id)
        {
            return await _repository.Get().Where(e => e.Id == id)
                                    .Include(e => e.SportEvent)
                                    .Include(e => e.Trainee)
                                    .ThenInclude(e => e.Group)
                                    .Include(e => e.Trainee)
                                    .ThenInclude(e => e.Group.SportType)
                                    .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<EventParticipant>> GetByEventIdAsync(int eventId)
        {
            return await _repository.GetAll().Where(p => p.SportEventId == eventId)
                                    .Include(e => e.SportEvent)
                                    .Include(e => e.Trainee)
                                    .ThenInclude(e => e.Group)
                                    .Include(e => e.Trainee)
                                    .ThenInclude(e => e.Group.SportType)
                                    .ToListAsync();
        }

        public async Task<IEnumerable<EventParticipant>> GetByTraineeIdAsync(int traineeId)
        {
            return await _repository.Get().Where(e => e.TraineeId == traineeId)
                                    .Include(e => e.SportEvent)
                                    .Include(e => e.Trainee)
                                    .ThenInclude(e => e.Group)
                                    .Include(e => e.Trainee)
                                    .ThenInclude(e => e.Group.SportType)
                                    .ToListAsync();
        }
    }
}
