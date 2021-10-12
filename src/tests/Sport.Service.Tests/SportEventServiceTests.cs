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
    public class SportEventServiceTests
    {
        private readonly Mock<IRepository<SportEvent>> _mockSportEventRepository;
        private readonly ISportEventService _sportEventService;

        public SportEventServiceTests()
        {
            _mockSportEventRepository = new Mock<IRepository<SportEvent>>();
            _sportEventService = new SportEventService(_mockSportEventRepository.Object);
        }

        [Fact]
        public async void AddAsync_WhenSportEventExist_ShouldThrowException()
        {
            var sportEvent = CreateSportEvent();

            _mockSportEventRepository.Setup(e => e.Get()).Returns(sportEvent.ToQueryable().BuildMock().Object);

            await Assert.ThrowsAsync<ResourceExistException>(() => _sportEventService.AddAsync(sportEvent));
        }

        [Fact]
        public async void AddAsync_WhenSportEventExist_ShouldReturnSportEvent()
        {
            var sportEvent = CreateSportEvent();
            var emptyList = Enumerable.Empty<SportEvent>().AsQueryable().BuildMock();

            _mockSportEventRepository.Setup(e => e.Get()).Returns(emptyList.Object);
            _mockSportEventRepository.Setup(e => e.AddAsync(sportEvent));

            var result = await _sportEventService.AddAsync(sportEvent);

            Assert.NotNull(result);
            Assert.True(result.Id == 1);
        }

        [Fact]
        public async void AddAsync_ShouldCallAddFromRepository_OnlyOnce()
        {
            var sportEvent = CreateSportEvent();
            var emptyList = Enumerable.Empty<SportEvent>().AsQueryable().BuildMock();

            _mockSportEventRepository.Setup(e => e.Get()).Returns(emptyList.Object);
            _mockSportEventRepository.Setup(e => e.AddAsync(sportEvent));

            await _sportEventService.AddAsync(sportEvent);

            _mockSportEventRepository.Verify(mock => mock.AddAsync(sportEvent), Times.Once);
        }

        [Fact]
        public async void UpdateAsync_WhenSportEventNotExist_ShouldThrowException()
        {
            var sportEvent = CreateSportEvent();
            var emptyList = Enumerable.Empty<SportEvent>().AsQueryable().BuildMock();

            _mockSportEventRepository.Setup(e => e.Get()).Returns(emptyList.Object);

            await Assert.ThrowsAsync<NotFoundException>(()
                                    => _sportEventService.UpdateAsync(1, sportEvent));
        }

        [Fact]
        public async void UpdateAsync_WhenSportEventExist_ShouldReturnSportEvent()
        {
            var sportEvent = CreateSportEvent();

            _mockSportEventRepository.Setup(e => e.Get()).Returns(sportEvent.ToQueryable().BuildMock().Object);
            _mockSportEventRepository.Setup(e => e.UpdateAsync(sportEvent));

            var result = await _sportEventService.UpdateAsync(1, sportEvent);

            Assert.NotNull(result);
            Assert.True(result.Id == 1);
        }

        [Fact]
        public async void UpdateAsync_ShouldCallAddFromRepository_OnlyOnce()
        {
            var sportEvent = CreateSportEvent();

            _mockSportEventRepository.Setup(e => e.Get()).Returns(sportEvent.ToQueryable().BuildMock().Object);
            _mockSportEventRepository.Setup(e => e.UpdateAsync(sportEvent));

            await _sportEventService.UpdateAsync(1, sportEvent);

            _mockSportEventRepository.Verify(mock => mock.UpdateAsync(sportEvent), Times.Once);
        }

        [Fact]
        public async void GetAllAsync_WhenSportEventNotExist_ShouldReturnEmptyList()
        {
            var sportEvent = CreateSportEvent();
            var emptyList = Enumerable.Empty<SportEvent>().AsQueryable().BuildMock();

            _mockSportEventRepository.Setup(e => e.GetAll()).Returns(emptyList.Object);

            var result = await _sportEventService.GetAllAsync();

            Assert.True(result.Count() == 0);
        }

        [Fact]
        public async void GetAllAsync_WhenSportEventExist_ShouldReturnSportEventList()
        {
            var sportEvents = CreateSportEvents();

            _mockSportEventRepository.Setup(e => e.GetAll()).Returns(sportEvents.AsQueryable().BuildMock().Object);

            var result = await _sportEventService.GetAllAsync();

            Assert.NotNull(result);
            Assert.True(result.Count() == 2);
        }

        private static List<SportEvent> CreateSportEvents()
        {
            return new List<SportEvent>
            {
                new SportEvent
                {
                    Id = 1,
                    ComplexId = 1,
                    Name = "A",
                    Description = "aa"                   
                },
                new SportEvent
                {
                    Id = 2,
                    ComplexId = 1,
                    Name = "B",
                    Description = "bb"
                }
            };
        }

        private static SportEvent CreateSportEvent()
        {
            return new SportEvent
            {
                Id = 1,
                ComplexId = 1,
                Name = "A",
                Description = "aa"
            };
        }
    }
}
