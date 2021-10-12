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
    public class EventSubscriberService : IEventSubscriberService
    {
        private readonly IRepository<EventSubscriber> _repository;

        public EventSubscriberService(IRepository<EventSubscriber> repository)
        {
            _repository = repository;
        }

        public async Task<EventSubscriber> AddAsync(EventSubscriber eventSubscriber)
        {
            await _repository.AddAsync(eventSubscriber);
            return eventSubscriber;
        }

        public async Task<EventSubscriber> DeleteAsync(int id)
        {
            var subscriber = await _repository.Get().Where(e => e.Id == id)
                                            .FirstOrDefaultAsync();

            if (subscriber == null)
                throw new NotFoundException("Not found");

            await _repository.DeleteAsync(subscriber);

            return subscriber;
        }

        public async Task<IEnumerable<EventSubscriber>> GetAllAsync()
        {
            return await _repository.GetAll().ToListAsync();
        }

        public async Task<EventSubscriber> GetAsync(int id)
        {
            return await _repository.Get().Where(e => e.Id == id).FirstOrDefaultAsync();
        }

        public async Task<EventSubscriber> UpdateAsync(int id, EventSubscriber eventSubscriber)
        {
            var subscriber = await _repository.Get().Where(e => e.Id == id).FirstOrDefaultAsync();

            if (subscriber == null)
                throw new NotFoundException("Not Found");

            subscriber.Url = eventSubscriber.Url;

            await _repository.UpdateAsync(subscriber);

            return subscriber;
        }
    }
}
