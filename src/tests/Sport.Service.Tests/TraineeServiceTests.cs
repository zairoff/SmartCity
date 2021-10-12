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
    public class TraineeServiceTests
    {
        private readonly Mock<IRepository<Trainee>> _mockTraineeRepository;
        private readonly ITraineeService _traineeService;

        public TraineeServiceTests()
        {
            _mockTraineeRepository = new Mock<IRepository<Trainee>>();
            _traineeService = new TraineeService(_mockTraineeRepository.Object);
        }

        [Fact]
        public async void AddAsync_WhenTraineeAlreadyExist_ShouldThrowException()
        {
            var trainee = CreateTrainee();

            _mockTraineeRepository.Setup(e => e.Get()).Returns(trainee.ToQueryable().BuildMock().Object);

            await Assert.ThrowsAsync<ResourceExistException>(()
                            => _traineeService.AddAsync(trainee));
        }

        [Fact]
        public async void AddAsync_WhenTraineeNotExist_ShouldReturnTrainee()
        {
            var trainee = CreateTrainee();
            var emptyList = Enumerable.Empty<Trainee>().AsQueryable().BuildMock();

            _mockTraineeRepository.Setup(e => e.Get()).Returns(emptyList.Object);
            _mockTraineeRepository.Setup(e => e.AddAsync(trainee));

            var result = await _traineeService.AddAsync(trainee);

            Assert.NotNull(result);
            Assert.True(result.Id == 1);
            Assert.IsType<Trainee>(result);
        }

        [Fact]
        public async void AddAsync_ShouldCallAddFromRepository_OnlyOnce()
        {
            var trainee = CreateTrainee();
            var emptyList = Enumerable.Empty<Trainee>().AsQueryable().BuildMock();

            _mockTraineeRepository.Setup(e => e.Get()).Returns(emptyList.Object);
            _mockTraineeRepository.Setup(e => e.AddAsync(trainee));

            await _traineeService.AddAsync(trainee);

            _mockTraineeRepository.Verify(mock => mock.AddAsync(trainee), Times.Once);
        }

        [Fact]
        public async void DeleteAsync_WhenTraineeNotExist_ShouldThrowException()
        {
            var emptyList = Enumerable.Empty<Trainee>().AsQueryable().BuildMock();

            _mockTraineeRepository.Setup(e => e.Get()).Returns(emptyList.Object);

            await Assert.ThrowsAsync<NotFoundException>(()
                            => _traineeService.DeleteAsync(1));
        }

        [Fact]
        public async void DeleteAsync_WhenTraineeExist_ShouldReturnTrainee()
        {
            var trainee = CreateTrainee();
            var emptyList = Enumerable.Empty<Trainee>().AsQueryable().BuildMock();

            _mockTraineeRepository.Setup(e => e.Get()).Returns(emptyList.Object);
            _mockTraineeRepository.Setup(e => e.AddAsync(trainee));

            var result = await _traineeService.AddAsync(trainee);

            Assert.NotNull(result);
            Assert.True(result.Id == 1);
            Assert.IsType<Trainee>(result);
        }

        [Fact]
        public async void DeleteAsync_ShouldCallAddFromRepository_OnlyOnce()
        {
            var trainee = CreateTrainee();

            _mockTraineeRepository.Setup(e => e.Get()).Returns(trainee.ToQueryable().BuildMock().Object);
            _mockTraineeRepository.Setup(e => e.DeleteAsync(trainee));

            await _traineeService.DeleteAsync(1);

            _mockTraineeRepository.Verify(mock => mock.DeleteAsync(trainee), Times.Once);
        }

        [Fact]
        public async void GetByPersonIdAsync_WhenTraineeExist_ShouldReturnTrainee()
        {
            var trainee = CreateTrainee();

            _mockTraineeRepository.Setup(e => e.Get()).Returns(trainee.ToQueryable().BuildMock().Object);

            var result = await _traineeService.GetByPersonIdAsync(1, "1");

            Assert.NotNull(result);
            Assert.True(result.Id == 1);
            Assert.IsType<Trainee>(result);
        }

        [Fact]
        public async void GetByPocketIdAsync_WhenTraineeExist_ShouldReturnTraineeList()
        {
            var trainees = CreateTrainees();

            _mockTraineeRepository.Setup(e => e.GetAll()).Returns(trainees.AsQueryable().BuildMock().Object);

            var result = await _traineeService.GetByPocketIdAsync(1, 1);

            Assert.NotNull(result);
            Assert.True(result.Count() == 2);
            Assert.IsType<List<Trainee>>(result);
        }

        private static List<Trainee> CreateTrainees()
        {
            return new List<Trainee>
            {
                new Trainee
                {
                    Id = 1,
                    ComplexId = 1,
                    FirstName = "A",
                    PocketId = 1
                },
                new Trainee
                {
                    Id = 2,
                    ComplexId = 1,
                    FirstName = "B",
                    PocketId = 1
                }
            };
        }

        private static Trainee CreateTrainee()
        {
            return new Trainee
            {
                Id = 1,
                ComplexId = 1,
                PersonId = "1",
                FirstName = "A"
            };
        }
    }
}
