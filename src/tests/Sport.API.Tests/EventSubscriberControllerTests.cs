using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using Sport.API.Controllers;
using Sport.API.DTOs.EventSubscriberDto;
using Sport.API.Mapping;
using Sport.Domain.Models;
using Sport.Service.Exceptions;
using Sport.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sport.API.Tests
{
    public class EventSubscriberControllerTests
    {
        private readonly Mock<IEventSubscriberService> _mockEventSubscriberService;
        private readonly EventSubscriberController _eventSubscriberController;
        private readonly IMapper _mockMapper;

        public EventSubscriberControllerTests()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ResourceToModelProfile());
                cfg.AddProfile(new ModelToResourceProfile());
            });

            _mockMapper = mapperConfig.CreateMapper();
            _mockEventSubscriberService = new Mock<IEventSubscriberService>();
            _eventSubscriberController = new EventSubscriberController(_mockEventSubscriberService.Object, _mockMapper, null);
        }

        [Fact]
        public async void GetAllAsync_WhenSubscribersExist_ShouldReturn_OkObjectResult_With_Resource()
        {
            var subscribers = CreateSubscribers();

            _mockEventSubscriberService.Setup(e => e.GetAllAsync())
                            .ReturnsAsync(subscribers);

            var result = await _eventSubscriberController.GetAllAsync();

            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<List<EventSubscriberResponseDto>>(okObjectResult.Value);

            Assert.NotNull(model);
            Assert.IsType<List<EventSubscriberResponseDto>>(model);
        }

        [Fact]
        public async void GetAsync_WhenSubscriberNotExist_ShouldReturn_NotFoundActionResult()
        {
            _mockEventSubscriberService.Setup(e => e.GetAsync(1))
                            .ReturnsAsync((EventSubscriber)null);

            var result = await _eventSubscriberController.GetAsync(1);

            var actionResult = (IStatusCodeActionResult)result;

            Assert.Equal(404, actionResult.StatusCode);
        }

        [Fact]
        public async void SubscribeAsync_WhenSubscriberNotExist_ShouldReturn_OkObjectResult_With_Resource()
        {
            var subscriberCreateDto = new EventSubscriberCreateDto { Url = "AAA" };

            var subscriber = CreateSubscriber();

            _mockEventSubscriberService.Setup(e => e.AddAsync(It.IsAny<EventSubscriber>()))
                                .ReturnsAsync(subscriber);

            var result = await _eventSubscriberController.SubscribeAsync(subscriberCreateDto);

            var createdAtActionResult = Assert.IsAssignableFrom<CreatedAtActionResult>(result);
            var model = Assert.IsAssignableFrom<EventSubscriberResponseDto>(createdAtActionResult.Value);

            Assert.NotNull(model);
            Assert.IsType<EventSubscriberResponseDto>(model);
        }

        [Fact]
        public async void DeleteAsync_WhenSubscriberExist_ShouldReturn_OkObjectResult_With_Resource()
        {
            _mockEventSubscriberService.Setup(e => e.DeleteAsync(1))
                                .ReturnsAsync(CreateSubscriber());

            var result = await _eventSubscriberController.DeleteAsync(1);

            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<EventSubscriberResponseDto>(okObjectResult.Value);

            Assert.NotNull(model);
            Assert.IsType<EventSubscriberResponseDto>(model);
        }

        [Fact]
        public async void DeleteAsync_WhenSubscriberNotExist_ShouldReturn_NotFoundActionResult()
        {
            _mockEventSubscriberService.Setup(e => e.DeleteAsync(1))
                                .Throws<NotFoundException>();

            var result = await _eventSubscriberController.DeleteAsync(1);

            var actionResult = (IStatusCodeActionResult)result;

            Assert.Equal(404, actionResult.StatusCode);
        }

        private static IEnumerable<EventSubscriber> CreateSubscribers()
        {
            return new List<EventSubscriber>
            {
                new EventSubscriber
                {
                    Id = 1,
                    Url = "aa"
                },
                new EventSubscriber
                {
                    Id = 2,
                    Url = "bb"
                }
            };
        }

        private static EventSubscriber CreateSubscriber()
        {
            return new EventSubscriber
            {
                Id = 1,
                Url = "bb"
            };
        }
    }
}
