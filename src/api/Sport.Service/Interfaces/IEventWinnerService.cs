using Sport.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sport.Service.Interfaces
{
    public interface IEventWinnerService
    {
        Task<IEnumerable<EventWinner>> GetAllAsync();

        Task<IEnumerable<EventWinner>> GetByEventIdAsync(int eventId);

        Task<EventWinner> GetAsync(int id);

        Task<EventWinner> GetByParticipantIdAsync(int participanId);

        Task<EventWinner> AddAsync(EventWinner eventWinner);

        Task<EventWinner> DeleteAsync(int id);
    }
}
