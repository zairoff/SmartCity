using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using Sport.API.Controllers;
using Sport.API.DTOs.ApplicantDto;
using Sport.API.Mapping;
using Sport.Domain.Models;
using Sport.Service.Exceptions;
using Sport.Service.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sport.API.Tests
{
    public class ApplicationControllerTests
    {
        private readonly Mock<IApplicantService> _mockApplicantService;
        private readonly ApplicantController _applicantController;
        private readonly IMapper _mockMapper;

        public ApplicationControllerTests()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ResourceToModelProfile());
                cfg.AddProfile(new ModelToResourceProfile());
            });

            _mockMapper = mapperConfig.CreateMapper();
            _mockApplicantService = new Mock<IApplicantService>();
            _applicantController = new ApplicantController(_mockApplicantService.Object, _mockMapper, null);
        }


        [Fact]
        public async void GetAsync_WhenApplicantExist_ShouldReturn_OkObjectResult_With_Resource()
        {
            var applicants = CreateApplicants();

            _mockApplicantService.Setup(e => e.GetAllAsync())
                        .ReturnsAsync(applicants);

            var result = await _applicantController.GetAsync();

            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<List<ApplicantResponseDto>>(okObjectResult.Value);
            Assert.Equal("1", model[0].PersonId);
            Assert.IsType<List<ApplicantResponseDto>>(model);
        }

        [Fact]
        public async void GetAsync_WhenApplicantNotExist_ShouldReturn_NotFoundIActionResult()
        {
            _mockApplicantService.Setup(e => e.GetAsync(1))
                            .ReturnsAsync((Applicant)null);

            var result = await _applicantController.GetAsync(1);

            var actionResult = (IStatusCodeActionResult)result;

            Assert.Equal(404, actionResult.StatusCode);
        }

        [Fact]
        public async void GetAsync_ById_WhenApplicantExist_ShouldReturn_OkObjectResult_With_Resource()
        {
            var applicant = CreateApplicant();

            _mockApplicantService.Setup(e => e.GetAsync(1)).Returns(Task.FromResult(applicant));

            var result = await _applicantController.GetAsync(1);

            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<ApplicantResponseDto>(okObjectResult.Value);
            Assert.Equal("1", model.PersonId);
            Assert.IsType<ApplicantResponseDto>(model);
        }

        [Fact]
        public async void GetAsync_ById_WhenApplicantNotExist_ShouldReturn_NotFoundResponse()
        {
            _mockApplicantService.Setup(e => e.GetAsync(1)).Returns(Task.FromResult((Applicant)null));

            var result = await _applicantController.GetAsync(1);

            var actionResult = (IStatusCodeActionResult)result;

            Assert.Equal(404, actionResult.StatusCode);
        }

        [Fact]
        public async void CreateAsync_WhenApplicantNotExist_ShouldReturn_OkObjectResult_With_Resource()
        {
            var applicantDto = new ApplicantCreateDto { PersonId = "1", VacancyId = 1 };

            var applicant = CreateApplicant();

            _mockApplicantService.Setup(e => e.AddAsync(It.IsAny<Applicant>()))
                                .ReturnsAsync(applicant);

            var result = await _applicantController.CreateAsync(applicantDto);

            var createdAtActionResult = Assert.IsAssignableFrom<CreatedAtActionResult>(result);
            var model = Assert.IsAssignableFrom<ApplicantResponseDto>(createdAtActionResult.Value);
            Assert.Equal("1", model.PersonId);
        }

        [Fact]
        public async void DeleteAsync_WhenApplicantExist_OkObjectResult_With_Resource()
        {
            var applicant = CreateApplicant();

            _mockApplicantService.Setup(e => e.DeleteAsync(1))
                                .ReturnsAsync(applicant);

            var result = await _applicantController.DeleteAsync(1);

            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<ApplicantResponseDto>(okObjectResult.Value);
            Assert.Equal("1", model.PersonId);
        }

        [Fact]
        public async void DeleteAsync_WhenApplicantNotExist_ThrowException()
        {
            var applicant = CreateApplicant();

            _mockApplicantService.Setup(e => e.DeleteAsync(1))
                                .Throws<NotFoundException>();

            var result = await _applicantController.DeleteAsync(1);

            var actionResult = (IStatusCodeActionResult)result;

            Assert.Equal(404, actionResult.StatusCode);
        }

        private static IEnumerable<Applicant> CreateApplicants()
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
                    Id = 2,
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
