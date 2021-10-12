using MockQueryable.Moq;
using Moq;
using Sport.Domain.Models;
using Sport.Infrastructure.Base;
using Sport.Service.Exceptions;
using Sport.Service.Interfaces;
using Sport.Service.Tests.Extensions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sport.Service.Tests
{
    public class PocketServiceTests
    {
        private readonly Mock<IRepository<Pocket>> _mockPocketRepository;
        private readonly IPocketService _pocketService;

        public PocketServiceTests()
        {
            _mockPocketRepository = new Mock<IRepository<Pocket>>();
            _pocketService = new PocketService(_mockPocketRepository.Object);
        }

        [Fact]
        public async void AddAsync_WhenPocketExist_ShouldThrowException()
        {
            var pocket = CreatePocket();

            _mockPocketRepository.Setup(e => e.Get()).Returns(pocket.ToQueryable().BuildMock().Object);

            await Assert.ThrowsAsync<ResourceExistException>(() => _pocketService.AddAsync(pocket));
        }

        [Fact]
        public async void AddAsync_WhenPocketNotExist_ShouldReturnPocket()
        {
            var pocket = CreatePocket();
            var emptyList = Enumerable.Empty<Pocket>().AsQueryable().BuildMock();

            _mockPocketRepository.Setup(e => e.Get()).Returns(emptyList.Object);
            _mockPocketRepository.Setup(e => e.AddAsync(pocket));

            var result = await _pocketService.AddAsync(pocket);

            Assert.NotNull(result);
            Assert.True(result.Id == 1);
        }

        [Fact]
        public async void AddAsync_ShouldCallAddFromRepository_OnlyOnce()
        {
            var pocket = CreatePocket();
            var emptyList = Enumerable.Empty<Pocket>().AsQueryable().BuildMock();

            _mockPocketRepository.Setup(e => e.Get()).Returns(emptyList.Object);
            _mockPocketRepository.Setup(e => e.AddAsync(pocket));

            await _pocketService.AddAsync(pocket);

            _mockPocketRepository.Verify(mock => mock.AddAsync(pocket), Times.Once);
        }

        [Fact]
        public async void DeleteAsync_WhenPocketNotExist_ShouldThrowException()
        {
            var pocket = CreatePocket();
            var emptyList = Enumerable.Empty<Pocket>().AsQueryable().BuildMock();

            _mockPocketRepository.Setup(e => e.Get()).Returns(emptyList.Object);
            _mockPocketRepository.Setup(e => e.DeleteAsync(pocket));                                

            await Assert.ThrowsAsync<NotFoundException>(() => _pocketService.DeleteAsync(1));
        }

        [Fact]
        public async void DeleteAsync_WhenPocketExist_ShouldReturnPocket()
        {
            var pocket = CreatePocket();
            var emptyList = Enumerable.Empty<Pocket>().AsQueryable().BuildMock();

            _mockPocketRepository.Setup(e => e.Get()).Returns(pocket.ToQueryable().BuildMock().Object);
            _mockPocketRepository.Setup(e => e.DeleteAsync(pocket));

            var result = await _pocketService.DeleteAsync(1);

            Assert.NotNull(result);
            Assert.True(result.Id == 1);
        }

        [Fact]
        public async void DeleteAsync_ShouldCallAddFromRepository_OnlyOnce()
        {
            var pocket = CreatePocket();

            _mockPocketRepository.Setup(e => e.Get()).Returns(pocket.ToQueryable().BuildMock().Object);
            _mockPocketRepository.Setup(e => e.DeleteAsync(pocket));

            await _pocketService.DeleteAsync(1);

            _mockPocketRepository.Verify(mock => mock.DeleteAsync(pocket), Times.Once);
        }

        private static Pocket CreatePocket()
        {
            return new Pocket
            {
                Id = 1,
                Name = "A",
                PricePerMonth = 1
            };
        }
    }
}
