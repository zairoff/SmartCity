using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using Sport.API.Controllers;
using Sport.API.DTOs.EventParticipantDto;
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
    public class EventParticipantControllerTests
    {
        private readonly Mock<IEventParticipantService> _mockEventParticipantService;
        private readonly EventParticipantController _eventParticipantController;
        private readonly IMapper _mockMapper;

        public EventParticipantControllerTests()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ResourceToModelProfile());
                cfg.AddProfile(new ModelToResourceProfile());
            });

            _mockMapper = mapperConfig.CreateMapper();
            _mockEventParticipantService = new Mock<IEventParticipantService>();
            _eventParticipantController = new EventParticipantController(_mockEventParticipantService.Object, _mockMapper, null);
        }

        [Fact]
        public async void GetAllAsync_WhenParticipantExist_ShouldReturn_OkObjectResult_With_Resource()
        {
            var partisipants = CreateParticipants();

            _mockEventParticipantService.Setup(e => e.GetAllAsync())
                            .ReturnsAsync(partisipants);

            var result = await _eventParticipantController.GetAsync();

            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<List<EventParticipantResponseDto>>(okObjectResult.Value);

            Assert.NotNull(model);
            Assert.IsType<List<EventParticipantResponseDto>>(model);
        }

        [Fact]
        public async void GetAsync_WhenParticipantExist_ShouldReturn_OkObjectResult_With_Resource()
        {
            var partisipant = CreateParticipant();

            _mockEventParticipantService.Setup(e => e.GetAsync(1))
                            .ReturnsAsync(partisipant);

            var result = await _eventParticipantController.GetAsync(1);

            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<EventParticipantResponseDto>(okObjectResult.Value);

            Assert.NotNull(model);
            Assert.IsType<EventParticipantResponseDto>(model);
        }

        [Fact]
        public async void CreateAsync_WhenParticipanNotExist_ShouldReturn_OkObjectResult_With_Resource()
        {
            var applicantDto = new EventParticipantCreateDto { SportEventId = 1, TraineeId = 1 };

            var applicant = CreateParticipant();

            _mockEventParticipantService.Setup(e => e.AddAsync(It.IsAny<EventParticipant>()))
                                .ReturnsAsync(applicant);

            var result = await _eventParticipantController.CreateAsync(applicantDto);

            var createdAtActionResult = Assert.IsAssignableFrom<CreatedAtActionResult>(result);
            var model = Assert.IsAssignableFrom<EventParticipantResponseDto>(createdAtActionResult.Value);

            Assert.NotNull(model);
            Assert.IsType<EventParticipantResponseDto>(model);
        }

        [Fact]
        public async void CreateAsync_WhenEventParticipant_ShouldReturn_NotFoundActionResult()
        {
            var applicantDto = new EventParticipantCreateDto { SportEventId = 1, TraineeId = 1 };

            _mockEventParticipantService.Setup(e => e.AddAsync(It.IsAny<EventParticipant>()))
                                .Throws<ResourceExistException>();

            var result = await _eventParticipantController.CreateAsync(applicantDto);

            var actionResult = (IStatusCodeActionResult)result;

            Assert.Equal(409, actionResult.StatusCode);
        }

        [Fact]
        public async void DeleteAsync_WhenEventParticipantExist_ShouldReturn_OkObjectResult_With_Resource()
        {
            var applicant = CreateParticipant();

            _mockEventParticipantService.Setup(e => e.DeleteAsync(1))
                                .ReturnsAsync(applicant);

            var result = await _eventParticipantController.DeleteAsync(1);

            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<EventParticipantResponseDto>(okObjectResult.Value);

            Assert.NotNull(model);
            Assert.IsType<EventParticipantResponseDto>(model);
        }

        [Fact]
        public async void DeleteAsync_WhenEventParticipantNotExist_ShouldReturn_NotFoundActionResult()
        {
            _mockEventParticipantService.Setup(e => e.DeleteAsync(1))
                                .Throws<NotFoundException>();

            var result = await _eventParticipantController.DeleteAsync(1);

            var actionResult = (IStatusCodeActionResult)result;

            Assert.Equal(404, actionResult.StatusCode);
        }

        private static IEnumerable<EventParticipant> CreateParticipants()
        {
            return new List<EventParticipant>
            {
                new EventParticipant
                {
                    Id = 1,
                    SportEventId = 1,
                    TraineeId = 1
                },
                new EventParticipant
                {
                    Id = 2,
                    SportEventId = 2,
                    TraineeId = 2
                }
            };
        }

        private static EventParticipant CreateParticipant()
        {
            return new EventParticipant
            {
                Id = 1,
                SportEventId = 1,
                TraineeId = 1
            };
        }
    }
}
