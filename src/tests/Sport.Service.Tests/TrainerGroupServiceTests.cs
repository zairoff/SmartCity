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
    public class TrainerGroupServiceTests
    {
        private readonly Mock<IRepository<TrainerGroup>> _mockTrainerGroupRepository;
        private readonly ITrainerGroupService _trainerGroupService;

        public TrainerGroupServiceTests()
        {
            _mockTrainerGroupRepository = new Mock<IRepository<TrainerGroup>>();
            _trainerGroupService = new TrainerGroupService(_mockTrainerGroupRepository.Object);
        }

        [Fact]
        public async void AddAsync_WhenTrainerAlreadyEnrolledToGroupExist_ShouldThrowException()
        {
            var group = CreateGroup();

            _mockTrainerGroupRepository.Setup(e => e.Get()).Returns(group.ToQueryable().BuildMock().Object);

            await Assert.ThrowsAsync<ResourceExistException>(()
                            => _trainerGroupService.AddAsync(group));
        }

        [Fact]
        public async void AddAsync_WhenTrainerNotEnrolledToGroup_ShouldReturnTrainerGroup()
        {
            var group = CreateGroup();
            var emptyList = Enumerable.Empty<TrainerGroup>().AsQueryable().BuildMock();

            _mockTrainerGroupRepository.Setup(e => e.Get()).Returns(emptyList.Object);
            _mockTrainerGroupRepository.Setup(a => a.AddAsync(group));

            var result = await _trainerGroupService.AddAsync(group);

            Assert.NotNull(result);
            Assert.True(result.Id == 1);
            Assert.IsType<TrainerGroup>(result);
        }

        [Fact]
        public async void AddAsync_ShouldCallAddFromRepository_OnlyOnce()
        {
            var group = CreateGroup();
            var emptyList = Enumerable.Empty<TrainerGroup>().AsQueryable().BuildMock();

            _mockTrainerGroupRepository.Setup(e => e.Get()).Returns(emptyList.Object);
            _mockTrainerGroupRepository.Setup(a => a.AddAsync(group));

            await _trainerGroupService.AddAsync(group);

            _mockTrainerGroupRepository.Verify(mock => mock.AddAsync(group), Times.Once);
        }

        [Fact]
        public async void DeleteAsync_WhenNotExist_ShouldThrowException()
        {
            var emptyList = Enumerable.Empty<TrainerGroup>().AsQueryable().BuildMock();

            _mockTrainerGroupRepository.Setup(e => e.Get()).Returns(emptyList.Object);

            await Assert.ThrowsAsync<NotFoundException>(()
                            => _trainerGroupService.DeleteAsync(1));
        }

        [Fact]
        public async void DeleteAsync_WhenExist_ShouldReturnTrainerGroup()
        {
            var group = CreateGroup();
            var emptyList = Enumerable.Empty<TrainerGroup>().AsQueryable().BuildMock();

            _mockTrainerGroupRepository.Setup(e => e.Get()).Returns(group.ToQueryable().BuildMock().Object);
            _mockTrainerGroupRepository.Setup(a => a.DeleteAsync(group));

            var result = await _trainerGroupService.DeleteAsync(1);

            Assert.NotNull(result);
            Assert.True(result.Id == 1);
            Assert.IsType<TrainerGroup>(result);
        }

        [Fact]
        public async void DeleteAsync_ShouldCallAddFromRepository_OnlyOnce()
        {
            var group = CreateGroup();

            _mockTrainerGroupRepository.Setup(e => e.Get()).Returns(group.ToQueryable().BuildMock().Object);
            _mockTrainerGroupRepository.Setup(a => a.DeleteAsync(group));

            await _trainerGroupService.DeleteAsync(1);

            _mockTrainerGroupRepository.Verify(mock => mock.DeleteAsync(group), Times.Once);
        }

        [Fact]
        public async void GetAllAsync_WhenExist_ShouldReturnListOfTrainerGroup()
        {
            var groups = CreateGroups();

            _mockTrainerGroupRepository.Setup(e => e.GetAll()).Returns(groups.AsQueryable().BuildMock().Object);

            var result = await _trainerGroupService.GetAllAsync();

            Assert.NotNull(result);
            Assert.True(result.Count() == 2);
            Assert.IsType<List<TrainerGroup>>(result);
        }

        [Fact]
        public async void GetAllAsync_WhenNotExist_ShouldReturnEmptyList()
        {
            var emptyList = Enumerable.Empty<TrainerGroup>().AsQueryable().BuildMock();

            _mockTrainerGroupRepository.Setup(e => e.GetAll()).Returns(emptyList.Object);

            var result = await _trainerGroupService.GetAllAsync();

            Assert.NotNull(result);
            Assert.True(result.Count() == 0);
            Assert.IsType<List<TrainerGroup>>(result);
        }

        [Fact]
        public async void GetByGroupIdAsync_WhenNotExist_ShouldReturnEmptyList()
        {
            var emptyList = Enumerable.Empty<TrainerGroup>().AsQueryable().BuildMock();

            _mockTrainerGroupRepository.Setup(e => e.GetAll()).Returns(emptyList.Object);

            var result = await _trainerGroupService.GetByGroupIdAsync(1);

            Assert.NotNull(result);
            Assert.True(result.Count() == 0);
            Assert.IsType<List<TrainerGroup>>(result);
        }

        [Fact]
        public async void GetByGroupIdAsync_WhenExist_ShouldReturnListOfTrainerGroup()
        {
            var groups = CreateGroups();

            _mockTrainerGroupRepository.Setup(e => e.GetAll()).Returns(groups.AsQueryable().BuildMock().Object);

            var result = await _trainerGroupService.GetByGroupIdAsync(1);

            Assert.NotNull(result);
            Assert.True(result.Count() == 2);
            Assert.IsType<List<TrainerGroup>>(result);
        }

        [Fact]
        public async void GetByTrainerIdAsync_WhenExist_ShouldReturnListOfTrainerGroup()
        {
            var groups = CreateGroups();

            _mockTrainerGroupRepository.Setup(e => e.GetAll()).Returns(groups.AsQueryable().BuildMock().Object);

            var result = await _trainerGroupService.GetByTrainerIdAsync(1);

            Assert.NotNull(result);
            Assert.True(result.Count() == 1);
            Assert.IsType<List<TrainerGroup>>(result);
        }

        [Fact]
        public async void GetByTrainerIdAsync_WhenNotExist_ShouldReturnEmptyList()
        {
            var emptyList = Enumerable.Empty<TrainerGroup>().AsQueryable().BuildMock();

            _mockTrainerGroupRepository.Setup(e => e.GetAll()).Returns(emptyList.Object);

            var result = await _trainerGroupService.GetByTrainerIdAsync(1);

            Assert.NotNull(result);
            Assert.True(result.Count() == 0);
            Assert.IsType<List<TrainerGroup>>(result);
        }

        private static List<TrainerGroup> CreateGroups()
        {
            return new List<TrainerGroup>
            {
                new TrainerGroup
                {
                    Id = 1,
                    GroupId = 1,
                    TrainerId = 1
                },
                new TrainerGroup
                {
                    Id = 2,
                    GroupId = 1,
                    TrainerId = 2
                }
            };
        }

        private static TrainerGroup CreateGroup()
        {
            return new TrainerGroup
            {
                Id = 1,
                GroupId = 1,
                TrainerId = 1
            };
        }
    }
}
