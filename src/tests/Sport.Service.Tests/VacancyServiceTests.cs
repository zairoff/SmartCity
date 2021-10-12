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
    public class VacancyServiceTests
    {
        private readonly Mock<IRepository<Vacancy>> _mockVacancyRepository;
        private readonly IVacancyService _vacancyService;

        public VacancyServiceTests()
        {
            _mockVacancyRepository = new Mock<IRepository<Vacancy>>();
            _vacancyService = new VacancyService(_mockVacancyRepository.Object);
        }

        [Fact]
        public async void AddAsync_ShouldAddAndReturnVacancy()
        {
            var vacancy = CreateVacancy();

            _mockVacancyRepository.Setup(e => e.AddAsync(vacancy));

            var result = await _vacancyService.AddAsync(vacancy);

            Assert.NotNull(result);
            Assert.True(result.Id == 1);
            Assert.IsType<Vacancy>(result);
        }

        [Fact]
        public async void DeleteAsync_WhenVacancyNotExist_ShouldThrowException()
        {
            var emptyList = Enumerable.Empty<Vacancy>().AsQueryable().BuildMock();

            _mockVacancyRepository.Setup(e => e.Get()).Returns(emptyList.Object);

            await Assert.ThrowsAsync<NotFoundException>(()
                            => _vacancyService.DeleteAsync(1));
        }

        [Fact]
        public async void DeleteAsync_WhenVacancyExist_ShouldReturnVacancy()
        {
            var vacancy = CreateVacancy();
            var emptyList = Enumerable.Empty<Trainee>().AsQueryable().BuildMock();

            _mockVacancyRepository.Setup(e => e.Get()).Returns(vacancy.ToQueryable().BuildMock().Object);
            _mockVacancyRepository.Setup(e => e.DeleteAsync(vacancy));

            var result = await _vacancyService.DeleteAsync(1);

            Assert.NotNull(result);
            Assert.True(result.Id == 1);
            Assert.IsType<Vacancy>(result);
        }

        [Fact]
        public async void DeleteAsync_ShouldCallAddFromRepository_OnlyOnce()
        {
            var vacancy = CreateVacancy();

            _mockVacancyRepository.Setup(e => e.Get()).Returns(vacancy.ToQueryable().BuildMock().Object);
            _mockVacancyRepository.Setup(e => e.DeleteAsync(vacancy));

            await _vacancyService.DeleteAsync(1);

            _mockVacancyRepository.Verify(mock => mock.DeleteAsync(vacancy), Times.Once);
        }

        private static Vacancy CreateVacancy()
        {
            return new Vacancy
            {
                Id = 1,
                ComplexId = 1,
                PositionId = 1
            };
        }
    }
}
