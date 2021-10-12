using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using Sport.API.Controllers;
using Sport.API.DTOs.EventWinnerDto;
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
    public class EventWinnerControllerTests
    {
        private readonly Mock<IEventWinnerService> _mockEventWinnerService;
        private readonly EventWinnerController _eventWinnerController;
        private readonly IMapper _mockMapper;

        public EventWinnerControllerTests()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ResourceToModelProfile());
                cfg.AddProfile(new ModelToResourceProfile());
            });

            _mockMapper = mapperConfig.CreateMapper();
            _mockEventWinnerService = new Mock<IEventWinnerService>();
            _eventWinnerController = new EventWinnerController(_mockEventWinnerService.Object, _mockMapper, null);
        }

        [Fact]
        public async void GetAllAsync_WhenWinnersExist_ShouldReturn_OkObjectResult_With_Resource()
        {
            var winners = CreateWinners();

            _mockEventWinnerService.Setup(e => e.GetAllAsync())
                            .ReturnsAsync(winners);

            var result = await _eventWinnerController.GetAsync();

            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<List<EventWinnerResponseDto>>(okObjectResult.Value);

            Assert.NotNull(model);
            Assert.IsType<List<EventWinnerResponseDto>>(model);
        }

        [Fact]
        public async void CreateAsync_WhenWinnerNotExist_ShouldReturn_OkObjectResult_With_Resource()
        {
            var winnerCreateDto = new EventWinnerCreateDto { ParticipantId = 1, Place = 1};

            var winner = CreateWinner();

            _mockEventWinnerService.Setup(e => e.AddAsync(It.IsAny<EventWinner>()))
                                .ReturnsAsync(winner);

            var result = await _eventWinnerController.CreateAsync(winnerCreateDto);

            var createdAtActionResult = Assert.IsAssignableFrom<CreatedAtActionResult>(result);
            var model = Assert.IsAssignableFrom<EventWinnerResponseDto>(createdAtActionResult.Value);

            Assert.NotNull(model);
            Assert.IsType<EventWinnerResponseDto>(model);
        }

        [Fact]
        public async void CreateAsync_WhenWinnerAlreadyExist_ShouldReturn_ConflictIActionResult()
        {
            var winnerCreateDto = new EventWinnerCreateDto { ParticipantId = 1, Place = 1 };

            _mockEventWinnerService.Setup(e => e.AddAsync(It.IsAny<EventWinner>()))
                                .Throws<ResourceExistException>();

            var result = await _eventWinnerController.CreateAsync(winnerCreateDto);

            var actionResult = (IStatusCodeActionResult)result;

            Assert.Equal(409, actionResult.StatusCode);
        }

        [Fact]
        public async void DeleteAsync_WhenWinnerNotExist_ShouldReturn_NotFoundIActionResult()
        {
            var winnerCreateDto = new EventWinnerCreateDto { ParticipantId = 1, Place = 1 };

            _mockEventWinnerService.Setup(e => e.DeleteAsync(1))
                                .Throws<NotFoundException>();

            var result = await _eventWinnerController.DeleteAsync(1);

            var actionResult = (IStatusCodeActionResult)result;

            Assert.Equal(404, actionResult.StatusCode);
        }

        [Fact]
        public async void DeleteAsync_WhenWinnerExist_ShouldReturn_OkObjectResult_With_Resource()
        {
            var winner = CreateWinner();

            _mockEventWinnerService.Setup(e => e.DeleteAsync(1))
                                .ReturnsAsync(winner);

            var result = await _eventWinnerController.DeleteAsync(1);

            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<EventWinnerResponseDto>(okObjectResult.Value);

            Assert.NotNull(model);
            Assert.IsType<EventWinnerResponseDto>(model);
        }


        private static IEnumerable<EventWinner> CreateWinners()
        {
            return new List<EventWinner>
            {
                new EventWinner
                {
                    Id = 1,
                    ParticipantId = 1,
                    Place = 1
                },
                new EventWinner
                {
                    Id = 2,
                    ParticipantId = 2,
                    Place = 2
                }
            };
        }

        private static EventWinner CreateWinner()
        {
            return new EventWinner
            {
                Id = 1,
                ParticipantId = 1,
                Place = 1
            };
        }
    }
}
