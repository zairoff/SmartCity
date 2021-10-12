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
    public class SportTypeServiceTests
    {
        private readonly Mock<IRepository<SportType>> _mockSportTypeRepository;
        private readonly ISportTypeService _sportTypeService;

        public SportTypeServiceTests()
        {
            _mockSportTypeRepository = new Mock<IRepository<SportType>>();
            _sportTypeService = new SportTypeService(_mockSportTypeRepository.Object);
        }

        [Fact]
        public async void AddAsync_WhenSportTypeExist_ShouldThrowException()
        {
            var sportType = CreateSportType();

            _mockSportTypeRepository.Setup(e => e.Get()).Returns(sportType.ToQueryable().BuildMock().Object);

            await Assert.ThrowsAsync<ResourceExistException>(() => _sportTypeService.AddAsync(sportType));
        }

        [Fact]
        public async void AddAsync_WhenSportTypeNotExist_ShouldReturnSportType()
        {
            var sportType = CreateSportType();
            var emptyList = Enumerable.Empty<SportType>().AsQueryable().BuildMock();

            _mockSportTypeRepository.Setup(e => e.Get()).Returns(emptyList.Object);
            _mockSportTypeRepository.Setup(e => e.AddAsync(sportType));

            var result = await _sportTypeService.AddAsync(sportType);

            Assert.NotNull(result);
            Assert.True(result.Id == 1);
        }

        [Fact]
        public async void AddAsync_ShouldCallAddFromRepository_OnlyOnce()
        {
            var sportEvent = CreateSportType();
            var emptyList = Enumerable.Empty<SportType>().AsQueryable().BuildMock();

            _mockSportTypeRepository.Setup(e => e.Get()).Returns(emptyList.Object);
            _mockSportTypeRepository.Setup(e => e.AddAsync(sportEvent));

            await _sportTypeService.AddAsync(sportEvent);

            _mockSportTypeRepository.Verify(mock => mock.AddAsync(sportEvent), Times.Once);
        }

        [Fact]
        public async void UpdateAsync_WhenSportTypeNotExist_ShouldThrowException()
        {
            var sportType = CreateSportType();
            var emptyList = Enumerable.Empty<SportType>().AsQueryable().BuildMock();

            _mockSportTypeRepository.Setup(e => e.Get()).Returns(emptyList.Object);
            _mockSportTypeRepository.Setup(e => e.UpdateAsync(sportType));

            await Assert.ThrowsAsync<NotFoundException>(()
                            => _sportTypeService.UpdateAsync(1, sportType));
        }

        [Fact]
        public async void DeleteAsync_WhenSportTypeExist_ShouldThrowException()
        {
            var emptyList = Enumerable.Empty<SportType>().AsQueryable().BuildMock();

            _mockSportTypeRepository.Setup(e => e.Get()).Returns(emptyList.Object);

            await Assert.ThrowsAsync<NotFoundException>(()
                            => _sportTypeService.DeleteAsync(1));
        }

        [Fact]
        public async void DeleteAsync_WhenSportTypeExist_ShouldReturnSportType()
        {
            var sportType = CreateSportType();

            _mockSportTypeRepository.Setup(e => e.Get()).Returns(sportType.ToQueryable().BuildMock().Object);
            _mockSportTypeRepository.Setup(e => e.DeleteAsync(sportType));

            var result = await _sportTypeService.DeleteAsync(1);

            Assert.NotNull(result);
            Assert.True(result.Id == 1);
            Assert.IsType<SportType>(result);
        }

        [Fact]
        public async void DeleteAsync_ShouldCallAddFromRepository_OnlyOnce()
        {
            var sportEvent = CreateSportType();

            _mockSportTypeRepository.Setup(e => e.Get()).Returns(sportEvent.ToQueryable().BuildMock().Object);
            _mockSportTypeRepository.Setup(e => e.DeleteAsync(sportEvent));

            await _sportTypeService.DeleteAsync(1);

            _mockSportTypeRepository.Verify(mock => mock.DeleteAsync(sportEvent), Times.Once);
        }

        private static SportType CreateSportType()
        {
            return new SportType
            {
                Id = 1,
                Name = "A"
            };
        }
    }
}
