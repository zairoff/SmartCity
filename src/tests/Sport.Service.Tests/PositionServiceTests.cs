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
    public class PositionServiceTests
    {
        private readonly Mock<IRepository<Position>> _mockPositionRepository;
        private readonly IPositionService _positionService;

        public PositionServiceTests()
        {
            _mockPositionRepository = new Mock<IRepository<Position>>();
            _positionService = new PositionService(_mockPositionRepository.Object);
        }

        [Fact]
        public async void AddAsync_WhenPositionExist_ShouldThrowException()
        {
            var position = CreatePosition();

            _mockPositionRepository.Setup(e => e.Get()).Returns(position.ToQueryable().BuildMock().Object);

            await Assert.ThrowsAsync<ResourceExistException>(() => _positionService.AddAsync(position));
        }

        [Fact]
        public async void AddAsync_WhenPositionNotExist_ShouldReturnPosition()
        {
            var position = CreatePosition();
            var emptyList = Enumerable.Empty<Position>().AsQueryable().BuildMock();

            _mockPositionRepository.Setup(e => e.Get()).Returns(emptyList.Object);
            _mockPositionRepository.Setup(e => e.AddAsync(position));

            var result = await _positionService.AddAsync(position);

            Assert.NotNull(result);
            Assert.True(result.Id == 1);
        }

        [Fact]
        public async void AddAsync_ShouldCallAddFromRepository_OnlyOnce()
        {
            var position = CreatePosition();
            var emptyList = Enumerable.Empty<Position>().AsQueryable().BuildMock();

            _mockPositionRepository.Setup(e => e.Get()).Returns(emptyList.Object);
            _mockPositionRepository.Setup(e => e.AddAsync(position));

            await _positionService.AddAsync(position);

            _mockPositionRepository.Verify(mock => mock.AddAsync(position), Times.Once);
        }

        [Fact]
        public async void GetAllAsync_WhenPositionExist_ShouldReturnPositionList()
        {
            var positions = CreatePositions();

            _mockPositionRepository.Setup(e => e.GetAll()).Returns(positions.AsQueryable().BuildMock().Object);

            var result = await _positionService.GetAllAsync();

            Assert.NotNull(result);
            Assert.True(result.Count() == 2);
        }

        [Fact]
        public async void GetAllAsync_WhenPositionNotExist_ShouldReturnEmptyList()
        {
            var emptyList = Enumerable.Empty<Position>().AsQueryable().BuildMock();

            _mockPositionRepository.Setup(e => e.GetAll()).Returns(emptyList.Object);

            var result = await _positionService.GetAllAsync();

            Assert.True(result.Count() == 0);
        }

        private static List<Position> CreatePositions()
        {
            return new List<Position>
            {
                new Position
                {
                    Id = 1,
                    Name = "A"
                },
                new Position
                {
                    Id = 2,
                    Name = "B"
                }
            };
        }

        private static Position CreatePosition()
        {
            return new Position
            {
                Id = 1,
                Name = "A"
            };
        }
    }
}
