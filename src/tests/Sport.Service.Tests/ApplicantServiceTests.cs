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
    public class ApplicantServiceTests
    {
        private readonly Mock<IRepository<Applicant>> _mockApplicantRepository;
        private readonly IApplicantService _applicantService;

        public ApplicantServiceTests()
        {
            _mockApplicantRepository = new Mock<IRepository<Applicant>>();
            _applicantService = new ApplicantService(_mockApplicantRepository.Object);
        }

        [Fact]
        public async void AddAsync_WhenApplicantExist_ShouldThrowException()
        {
            var applicant = CreateApplicant();

            _mockApplicantRepository.Setup(e => e.Get()).Returns(applicant.ToQueryable().BuildMock().Object);

            await Assert.ThrowsAsync<ResourceExistException>(() => _applicantService.AddAsync(applicant));
        }

        [Fact]
        public async void AddAsync_WhenApplicantNotExist_ShouldReturnApplicant()
        {
            var applicant = CreateApplicant();
            var emptyList = Enumerable.Empty<Applicant>().AsQueryable().BuildMock();

            _mockApplicantRepository.Setup(e => e.Get()).Returns(emptyList.Object);
            _mockApplicantRepository.Setup(a => a.AddAsync(applicant));

            var result = await _applicantService.AddAsync(applicant);

            Assert.NotNull(result);
            Assert.True(result.Id == 1);

        }

        [Fact]
        public async void AddAsync_ShouldCallAddFromRepository_OnlyOnce()
        {
            var applicant = CreateApplicant();
            var emptyList = Enumerable.Empty<Applicant>().AsQueryable().BuildMock();

            _mockApplicantRepository.Setup(e => e.Get()).Returns(emptyList.Object);
            _mockApplicantRepository.Setup(e => e.AddAsync(applicant));

            await _applicantService.AddAsync(applicant);

            _mockApplicantRepository.Verify(mock => mock.AddAsync(applicant), Times.Once);
        }

        [Fact]
        public async void DeleteAsync_WhenApplicantNotExist_ShouldThrowException()
        {
            var emptyList = Enumerable.Empty<Applicant>().AsQueryable().BuildMock();

            _mockApplicantRepository.Setup(e => e.Get()).Returns(emptyList.Object);

            await Assert.ThrowsAsync<NotFoundException>(() => _applicantService.DeleteAsync(1));
        }

        [Fact]
        public async void DeleteAsync_WhenApplicantExist_ShouldReturnApplicant()
        {
            var applicant = CreateApplicant();

            _mockApplicantRepository.Setup(e => e.Get())
                                    .Returns(applicant.ToQueryable().BuildMock().Object);
            _mockApplicantRepository.Setup(a => a.DeleteAsync(applicant));

            var result = await _applicantService.DeleteAsync(1);

            Assert.NotNull(result);
            Assert.True(result.Id == 1);
        }

        [Fact]
        public async void DeleteAsync_ShouldCallAddFromRepository_OnlyOnce()
        {
            var applicant = CreateApplicant();

            _mockApplicantRepository.Setup(e => e.Get())
                                    .Returns(applicant.ToQueryable().BuildMock().Object);
            _mockApplicantRepository.Setup(e => e.DeleteAsync(applicant));

            await _applicantService.DeleteAsync(1);

            _mockApplicantRepository.Verify(mock => mock.DeleteAsync(applicant), Times.Once);
        }

        [Fact]
        public async void GetAllAsync_WhenApplicantExist_ShouldReturnApplicantList()
        {
            var applicants = CreateApplicants();

            _mockApplicantRepository.Setup(e => e.GetAll())
                                    .Returns(applicants.AsQueryable().BuildMock().Object);

            var result = await _applicantService.GetAllAsync();

            Assert.NotNull(result);
            Assert.True(result.Count() == 2);
            Assert.IsType<List<Applicant>>(result);
        }

        [Fact]
        public async void GetAllAsync_WhenApplicantNotExist_ShouldReturnEmptyList()
        {
            var emptyList = Enumerable.Empty<Applicant>().AsQueryable().BuildMock();

            _mockApplicantRepository.Setup(e => e.GetAll())
                                    .Returns(emptyList.Object);

            var result = await _applicantService.GetAllAsync();

            Assert.True(result.Count() == 0);
        }

        private static List<Applicant> CreateApplicants()
        {
            return new List<Applicant>
            {
                new Applicant
                {
                    Id = 1,
                    PersonId = "1",
                    VacancyId = 1
                },
                new Applicant
                {
                    Id = 1,
                    PersonId = "1",
                    VacancyId = 1
                }
            };            
        }

        private static Applicant CreateApplicant()
        {
            return new Applicant
            {
                Id = 1,
                PersonId = "1",
                VacancyId = 1
            };
        }
    }
}
