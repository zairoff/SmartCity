using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using Sport.API.Controllers;
using Sport.API.DTOs.VacancyDto;
using Sport.API.Mapping;
using Sport.Domain.Models;
using Sport.Service.Exceptions;
using Sport.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sport.API.Tests
{
    public class VacancyControllerTests
    {
        private readonly Mock<IVacancyService> _mockVacancyService;
        private readonly VacancyController _vacancyController;
        private readonly IMapper _mockMapper;

        public VacancyControllerTests()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ResourceToModelProfile());
                cfg.AddProfile(new ModelToResourceProfile());
            });

            _mockMapper = mapperConfig.CreateMapper();
            _mockVacancyService = new Mock<IVacancyService>();
            _vacancyController = new VacancyController(_mockVacancyService.Object, _mockMapper, null);
        }

        [Fact]
        public async void GetAsync_WhenVacancyExist_ShouldReturn_OkObjectResult_With_Resource()
        {
            _mockVacancyService.Setup(e => e.GetAllAsync())
                                .ReturnsAsync(It.IsAny<List<Vacancy>>());

            var result = await _vacancyController.GetAsync();

            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<List<VacancyResponseDto>>(okObjectResult.Value);

            Assert.NotNull(model);
            Assert.IsType<List<VacancyResponseDto>>(model);
        }

        [Fact]
        public async void GetByPositionIdAsync_WhenVacancyExist_ShouldReturn_OkObjectResult_With_Resource()
        {
            _mockVacancyService.Setup(e => e.GetByPositionIdAsync(1, 1))
                                .ReturnsAsync(It.IsAny<List<Vacancy>>());

            var result = await _vacancyController.GetByPositionIdAsync(1, 1);

            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<List<VacancyResponseDto>>(okObjectResult.Value);

            Assert.NotNull(model);
            Assert.IsType<List<VacancyResponseDto>>(model);
        }

        [Fact]
        public async void GetByStatusAsync_WhenVacancyExist_ShouldReturn_OkObjectResult_With_Resource()
        {
            _mockVacancyService.Setup(e => e.GetByStatusAsync(1, true))
                                .ReturnsAsync(It.IsAny<List<Vacancy>>());

            var result = await _vacancyController.GetByStatusAsync(1, true);

            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<List<VacancyResponseDto>>(okObjectResult.Value);

            Assert.NotNull(model);
            Assert.IsType<List<VacancyResponseDto>>(model);
        }

        [Fact]
        public async void CreateAsync_ShouldAddAndReturn_OkObjectResult_With_Resource()
        {
            var vacancy = CreateVacancy();

            _mockVacancyService.Setup(e => e.AddAsync(It.IsAny<Vacancy>()))
                                .ReturnsAsync(vacancy);

            var result = await _vacancyController.CreateAsync(It.IsAny<VacancyCreateDto>());

            var createdAtActionResult = Assert.IsAssignableFrom<CreatedAtActionResult>(result);
            var model = Assert.IsAssignableFrom<VacancyResponseDto>(createdAtActionResult.Value);

            Assert.NotNull(model);
            Assert.IsType<VacancyResponseDto>(model);
        }

        [Fact]
        public async void CreateAsync_WhenVacancyAlreadyExist_ShouldReturn_ConflictIActionResult()
        {
            _mockVacancyService.Setup(e => e.AddAsync(It.IsAny<Vacancy>()))
                                .Throws<ResourceExistException>();

            var result = await _vacancyController.CreateAsync(It.IsAny<VacancyCreateDto>());

            var actionResult = (IStatusCodeActionResult)result;

            Assert.Equal(409, actionResult.StatusCode);
        }

        [Fact]
        public async void DeleteAsync_WhenTrainerGroupExist_ShouldReturn_OkObjectResult_With_Resource()
        {
            var vacancy = CreateVacancy();

            _mockVacancyService.Setup(e => e.DeleteAsync(1))
                                .ReturnsAsync(vacancy);

            var result = await _vacancyController.DeleteAsync(1);

            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<VacancyResponseDto>(okObjectResult.Value);

            Assert.NotNull(model);
            Assert.IsType<VacancyResponseDto>(model);
        }

        [Fact]
        public async void DeleteAsync_WhenTrainerGroupNotExist_ShouldReturn_ConflictIActionResult()
        {
            _mockVacancyService.Setup(e => e.DeleteAsync(1))
                                .Throws<NotFoundException>();

            var result = await _vacancyController.DeleteAsync(1);

            var actionResult = (IStatusCodeActionResult)result;

            Assert.Equal(404, actionResult.StatusCode);
        }


        private static Vacancy CreateVacancy()
        {
            return new Vacancy
            {
                Id = 1,
                ComplexId = 1,
                PositionId = 1,
                Title = "A",
                Details = "BB",
                IsActive = true
            };
        }
    }
}
