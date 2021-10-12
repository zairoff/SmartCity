using Microsoft.EntityFrameworkCore;
using Sport.Domain.Models;
using Sport.Infrastructure.Base;
using Sport.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sport.Service
{
    public class NotificationService : INotificationService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IRepository<EventSubscriber> _repository;

        public NotificationService(IHttpClientFactory clientFactory, IRepository<EventSubscriber> repository)
        {
            _clientFactory = clientFactory;
            _repository = repository;
        }

        public async Task SendAsync(SportEvent sportEvent)
        {
            var subscribers = await _repository.GetAll().ToListAsync();

            foreach (var subscriber in subscribers)
                await SendAsync(sportEvent, subscriber.Url);

        }

        private async Task<bool> SendAsync(SportEvent sportEvent, string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(JsonSerializer.Serialize(sportEvent), Encoding.UTF8, "application/json")
            };

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
                return true;

            return false;                    
        }
    }
}
