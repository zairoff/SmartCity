using Sport.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sport.Service.Interfaces
{
    public interface IEventParticipantService
    {
        Task<IEnumerable<EventParticipant>> GetAllAsync();

        Task<IEnumerable<EventParticipant>> GetByEventIdAsync(int eventId);

        Task<EventParticipant> GetAsync(int id);

        Task<IEnumerable<EventParticipant>> GetByTraineeIdAsync(int traineeId);

        Task<EventParticipant> AddAsync(EventParticipant eventParticipant);

        Task<EventParticipant> DeleteAsync(int id);
    }
}
