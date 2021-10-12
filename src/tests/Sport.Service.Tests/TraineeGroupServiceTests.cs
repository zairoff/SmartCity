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
    public class TraineeGroupServiceTests
    {
        private readonly Mock<IRepository<SportGroup>> _mockTraineeGroupRepository;
        private readonly ISportGroupService _traineeGroupService;

        public TraineeGroupServiceTests()
        {
            _mockTraineeGroupRepository = new Mock<IRepository<SportGroup>>();
            _traineeGroupService = new SportGroupService(_mockTraineeGroupRepository.Object);
        }

        [Fact]
        public async void AddAsync_WhenTraineeGroupAlreadyExist_ShouldThrowException()
        {
            var traineeGroup = CreateTraineeGroup();

            _mockTraineeGroupRepository.Setup(e => e.Get()).Returns(traineeGroup.ToQueryable().BuildMock().Object);

            await Assert.ThrowsAsync<ResourceExistException>(()
                            => _traineeGroupService.AddAsync(traineeGroup));
        }

        [Fact]
        public async void AddAsync_WhenTraineeGroupNotExist_ShouldReturnTraineeGroup()
        {
            var traineeGroup = CreateTraineeGroup();
            var emptyList = Enumerable.Empty<SportGroup>().AsQueryable().BuildMock();

            _mockTraineeGroupRepository.Setup(e => e.Get()).Returns(emptyList.Object);
            _mockTraineeGroupRepository.Setup(e => e.AddAsync(traineeGroup));

            var result = await _traineeGroupService.AddAsync(traineeGroup);

            Assert.NotNull(result);
            Assert.True(result.Id == 1);
            Assert.IsType<SportGroup>(result);
        }

        [Fact]
        public async void AddAsync_ShouldCallAddFromRepository_OnlyOnce()
        {
            var traineeGroup = CreateTraineeGroup();
            var emptyList = Enumerable.Empty<SportGroup>().AsQueryable().BuildMock();

            _mockTraineeGroupRepository.Setup(e => e.Get()).Returns(emptyList.Object);
            _mockTraineeGroupRepository.Setup(e => e.AddAsync(traineeGroup));

            await _traineeGroupService.AddAsync(traineeGroup);

            _mockTraineeGroupRepository.Verify(mock => mock.AddAsync(traineeGroup), Times.Once);
        }

        [Fact]
        public async void DeleteAsync_WhenTraineeGroupNotExist_ShouldThrowException()
        {
            var traineeGroup = CreateTraineeGroup();
            var emptyList = Enumerable.Empty<SportGroup>().AsQueryable().BuildMock();

            _mockTraineeGroupRepository.Setup(e => e.Get()).Returns(emptyList.Object);

            await Assert.ThrowsAsync<NotFoundException>(()
                            => _traineeGroupService.DeleteAsync(1));
        }

        [Fact]
        public async void DeleteAsync_WhenTraineeGroupExist_ShouldReturnTraineeGroup()
        {
            var traineeGroup = CreateTraineeGroup();

            _mockTraineeGroupRepository.Setup(e => e.Get()).Returns(traineeGroup.ToQueryable().BuildMock().Object);
            _mockTraineeGroupRepository.Setup(e => e.DeleteAsync(traineeGroup));

            var result = await _traineeGroupService.DeleteAsync(1);

            Assert.NotNull(result);
            Assert.True(result.Id == 1);
            Assert.IsType<SportGroup>(result);
        }

        [Fact]
        public async void DeleteAsync_ShouldCallAddFromRepository_OnlyOnce()
        {
            var traineeGroup = CreateTraineeGroup();

            _mockTraineeGroupRepository.Setup(e => e.Get()).Returns(traineeGroup.ToQueryable().BuildMock().Object);
            _mockTraineeGroupRepository.Setup(e => e.DeleteAsync(traineeGroup));

            await _traineeGroupService.DeleteAsync(1);

            _mockTraineeGroupRepository.Verify(mock => mock.DeleteAsync(traineeGroup), Times.Once);
        }

        private static SportGroup CreateTraineeGroup()
        {
            return new SportGroup
            {
                Id = 1,
                Name = "A"
            };
        }
    }
}
