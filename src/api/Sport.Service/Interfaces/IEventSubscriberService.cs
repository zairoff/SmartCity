using Sport.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sport.Service.Interfaces
{
    public interface IEventSubscriberService
    {
        Task<IEnumerable<EventSubscriber>> GetAllAsync();

        Task<EventSubscriber> GetAsync(int id);

        Task<EventSubscriber> AddAsync(EventSubscriber eventSubscriber);

        Task<EventSubscriber> UpdateAsync(int id, EventSubscriber eventSubscriber);

        Task<EventSubscriber> DeleteAsync(int id);
    }
}
