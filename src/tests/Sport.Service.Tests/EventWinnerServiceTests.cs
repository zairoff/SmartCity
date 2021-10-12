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
    public class EventWinnerServiceTests
    {
        private readonly Mock<IRepository<EventWinner>> _mockEventWinnerRepository;
        private readonly IEventWinnerService _eventWinnerService;

        public EventWinnerServiceTests()
        {
            _mockEventWinnerRepository = new Mock<IRepository<EventWinner>>();
            _eventWinnerService = new EventWinnerService(_mockEventWinnerRepository.Object);
        }

        [Fact]
        public async void DeleteAsync_WhenWinnerNotExist_ShouldThrowException()
        {
            var emptyList = Enumerable.Empty<EventWinner>().AsQueryable().BuildMock();

            _mockEventWinnerRepository.Setup(e => e.Get()).Returns(emptyList.Object);

            await Assert.ThrowsAsync<NotFoundException>(()
                                => _eventWinnerService.DeleteAsync(1));
        }

        [Fact]
        public async void DeleteAsync_WhenWinnerExist_ShouldReturnWinner()
        {
            var winner = CreateWinner();

            _mockEventWinnerRepository.Setup(e => e.Get()).Returns(winner.ToQueryable().BuildMock().Object);
            _mockEventWinnerRepository.Setup(a => a.DeleteAsync(winner));

            var result = await _eventWinnerService.DeleteAsync(1);

            Assert.NotNull(result);
            Assert.True(result.Id == 1);
            Assert.IsType<EventWinner>(result);
        }

        [Fact]
        public async void DeleteAsync_ShouldCallAddFromRepository_OnlyOnce()
        {
            var winner = CreateWinner();

            _mockEventWinnerRepository.Setup(e => e.Get()).Returns(winner.ToQueryable().BuildMock().Object);
            _mockEventWinnerRepository.Setup(a => a.DeleteAsync(winner));

            await _eventWinnerService.DeleteAsync(1);

            _mockEventWinnerRepository.Verify(mock => mock.DeleteAsync(winner), Times.Once);
        }

        [Fact]
        public async void GetAllAsync_WhenWinnerExist_ShouldReturnWinnerList()
        {
            var applicants = CreateWinners();

            _mockEventWinnerRepository.Setup(e => e.GetAll()).Returns(applicants.AsQueryable().BuildMock().Object);

            var result = await _eventWinnerService.GetAllAsync();

            Assert.NotNull(result);
            Assert.True(result.Count() == 2);
            Assert.IsType<List<EventWinner>>(result);
        }

        [Fact]
        public async void GetAllAsync_WhenWinnerNotExist_ShouldReturnEmptyList()
        {
            var emptyList = Enumerable.Empty<EventWinner>().AsQueryable().BuildMock();

            _mockEventWinnerRepository.Setup(e => e.GetAll()).Returns(emptyList.Object);

            var result = await _eventWinnerService.GetAllAsync();

            Assert.NotNull(result);
            Assert.True(result.Count() == 0);
            Assert.IsType<List<EventWinner>>(result);
        }

        private static List<EventWinner> CreateWinners()
        {
            return new List<EventWinner>
            {
                new EventWinner
                {
                    Id = 1,
                    Participant = new EventParticipant
                    {
                        Id = 1,
                        SportEventId = 1,
                        TraineeId = 1
                    },
                    Place = 1
                },
                new EventWinner
                {
                    Id = 2,
                    Participant = new EventParticipant
                    {
                        Id = 2,
                        SportEventId = 1,
                        TraineeId = 2
                    },
                    Place = 1
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
