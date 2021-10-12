using MockQueryable.Moq;
using Moq;
using Sport.Domain.Models;
using Sport.Infrastructure.Base;
using Sport.Service.Exceptions;
using Sport.Service.Interfaces;
using Sport.Service.Tests.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sport.Service.Tests
{
    public class EventParticipantServiceTests
    {
        private readonly Mock<IRepository<EventParticipant>> _mockEventParticipantRepository;
        private readonly IEventParticipantService _eventParticipantService;

        public EventParticipantServiceTests()
        {
            _mockEventParticipantRepository = new Mock<IRepository<EventParticipant>>();
            _eventParticipantService = new EventParticipantService(_mockEventParticipantRepository.Object);
        }

        [Fact]
        public async void AddAsync_WhenParticipantExist_ShouldThrowException()
        {
            var participant = CreateParticipant();

            _mockEventParticipantRepository.Setup(e => e.Get())
                                            .Returns(participant.ToQueryable().BuildMock().Object);

            await Assert.ThrowsAsync<ResourceExistException>(() => _eventParticipantService.AddAsync(participant));
        }

        [Fact]
        public async void AddAsync_WhenParticipantNotExist_ShouldReturnParticipant()
        {
            var participant = CreateParticipant();
            var emptyList = Enumerable.Empty<EventParticipant>().AsQueryable().BuildMock();

            _mockEventParticipantRepository.Setup(e => e.Get()).Returns(emptyList.Object);
            _mockEventParticipantRepository.Setup(a => a.AddAsync(participant));

            var result = await _eventParticipantService.AddAsync(participant);

            Assert.NotNull(result);
            Assert.True(result.Id == 1);
            Assert.IsType<EventParticipant>(result);
        }

        [Fact]
        public async void AddAsync_ShouldCallAddFromRepository_OnlyOnce()
        {
            var participant = CreateParticipant();
            var emptyList = Enumerable.Empty<EventParticipant>().AsQueryable().BuildMock();

            _mockEventParticipantRepository.Setup(e => e.Get()).Returns(emptyList.Object);
            _mockEventParticipantRepository.Setup(e => e.AddAsync(participant));

            await _eventParticipantService.AddAsync(participant);

            _mockEventParticipantRepository.Verify(mock => mock.AddAsync(participant), Times.Once);
        }

        [Fact]
        public async void GetAllAsync_WhenParticipantNotExist_ShouldReturnEmptyList()
        {
            var emptyList = Enumerable.Empty<EventParticipant>().AsQueryable().BuildMock();

            _mockEventParticipantRepository.Setup(e => e.GetAll()).Returns(emptyList.Object);

            var result = await _eventParticipantService.GetAllAsync();

            Assert.NotNull(result);
            Assert.True(result.Count() == 0);
            Assert.IsType<List<EventParticipant>>(result);
        }

        [Fact]
        public async void GetByTraineeIdAsync_WhenParticipantNotExist_ShouldReturnEmptyList()
        {
            var emptyList = Enumerable.Empty<EventParticipant>().AsQueryable().BuildMock();

            _mockEventParticipantRepository.Setup(e => e.Get()).Returns(emptyList.Object);

            var result = await _eventParticipantService.GetByTraineeIdAsync(1);

            Assert.NotNull(result);
            Assert.True(result.Count() == 0);
            Assert.IsType<List<EventParticipant>>(result);
        }

        [Fact]
        public async void GetByTraineeIdAsync_WhenParticipantExist_ShouldReturnEventParticipantList()
        {
            var participants = CreateParticipants().AsQueryable().BuildMock();

            _mockEventParticipantRepository.Setup(e => e.Get()).Returns(participants.Object);

            var result = await _eventParticipantService.GetByTraineeIdAsync(1);

            Assert.NotNull(result);
            Assert.True(result.Count() == 1);
            Assert.IsType<List<EventParticipant>>(result);
        }

        private static List<EventParticipant> CreateParticipants()
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
