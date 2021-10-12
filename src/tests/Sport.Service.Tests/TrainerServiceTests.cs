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
    public class TrainerServiceTests
    {
        private readonly Mock<IRepository<Trainer>> _mockTrainerRepository;
        private readonly ITrainerService _trainerService;

        public TrainerServiceTests()
        {
            _mockTrainerRepository = new Mock<IRepository<Trainer>>();
            _trainerService = new TrainerService(_mockTrainerRepository.Object);
        }

        [Fact]
        public async void AddAsync_WhenTrainerAlreadyExist_ShouldThrowException()
        {
            var trainer = CreateTrainer();

            _mockTrainerRepository.Setup(e => e.Get()).Returns(trainer.ToQueryable().BuildMock().Object);

            await Assert.ThrowsAsync<ResourceExistException>(()
                            => _trainerService.AddAsync(trainer));
        }

        [Fact]
        public async void AddAsync_WhenTrainerNotExist_ShouldReturnTrainer()
        {
            var trainer = CreateTrainer();
            var emptyList = Enumerable.Empty<Trainer>().AsQueryable().BuildMock();

            _mockTrainerRepository.Setup(e => e.Get()).Returns(emptyList.Object);
            _mockTrainerRepository.Setup(e => e.AddAsync(trainer));

            var result = await _trainerService.AddAsync(trainer);

            Assert.NotNull(result);
            Assert.True(result.Id == 1);
            Assert.IsType<Trainer>(result);
        }

        [Fact]
        public async void AddAsync_ShouldCallAddFromRepository_OnlyOnce()
        {
            var trainer = CreateTrainer();
            var emptyList = Enumerable.Empty<Trainer>().AsQueryable().BuildMock();

            _mockTrainerRepository.Setup(e => e.Get()).Returns(emptyList.Object);
            _mockTrainerRepository.Setup(e => e.AddAsync(trainer));

            await _trainerService.AddAsync(trainer);

            _mockTrainerRepository.Verify(mock => mock.AddAsync(trainer), Times.Once);
        }

        [Fact]
        public async void GetAllAsync_WhenTrainerExist_ShouldReturnTrainerList()
        {
            var trainers = CreateTrainers();

            _mockTrainerRepository.Setup(e => e.GetAll()).Returns(trainers.AsQueryable().BuildMock().Object);

            var result = await _trainerService.GetAllAsync();

            Assert.NotNull(result);
            Assert.True(result.Count() == 2);
            Assert.IsType<List<Trainer>>(result);
        }

        private static List<Trainer> CreateTrainers()
        {
            return new List<Trainer>
            {
                new Trainer
                {
                    Id = 1,
                    ComplexId = 1,
                    EmployeeId = 1,
                    SportTypeId = 1                  
                },
                new Trainer
                {
                    Id = 2,
                    ComplexId = 2,
                    EmployeeId = 2,
                    SportTypeId = 2
                }
            };
        }

        private static Trainer CreateTrainer()
        {
            return new Trainer
            {
                Id = 1,
                ComplexId = 1,
                EmployeeId = 1,
                SportTypeId = 1
            };
        }
    }
}
