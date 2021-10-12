using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using Sport.API.Controllers;
using Sport.API.DTOs.TraineeDto;
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
    public class TraineeControllerTests
    {
        private readonly Mock<ITraineeService> _mockTraineeService;
        private readonly TraineeController _traineeController;
        private readonly IMapper _mockMapper;

        public TraineeControllerTests()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ResourceToModelProfile());
                cfg.AddProfile(new ModelToResourceProfile());
            });

            _mockMapper = mapperConfig.CreateMapper();
            _mockTraineeService = new Mock<ITraineeService>();
            _traineeController = new TraineeController(_mockTraineeService.Object, _mockMapper, null);
        }

        [Fact]
        public async void GetAllAsync_WhenTraineeExist_ShouldReturn_OkObjectResult_With_Resource()
        {
            _mockTraineeService.Setup(e => e.GetAllAsync())
                                .ReturnsAsync(It.IsAny<List<Trainee>>());

            var result = await _traineeController.GetAsync();

            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<List<TraineeResponseDto>>(okObjectResult.Value);

            Assert.NotNull(model);
            Assert.IsType<List<TraineeResponseDto>>(model);
        }

        [Fact]
        public async void GetByGroupIdAsync_WhenTraineeExist_ShouldReturn_OkObjectResult_With_Resource()
        {
            _mockTraineeService.Setup(e => e.GetByGroupIdAsync(1, 1))
                                .ReturnsAsync(It.IsAny<List<Trainee>>());

            var result = await _traineeController.GetByGroupIdAsync(1, 1);

            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<List<TraineeResponseDto>>(okObjectResult.Value);

            Assert.NotNull(model);
            Assert.IsType<List<TraineeResponseDto>>(model);
        }

        [Fact]
        public async void GetByPaymentStatusAsync_WhenTraineeExist_ShouldReturn_OkObjectResult_With_Resource()
        {
            _mockTraineeService.Setup(e => e.GetByPaymentStatusAsync(1, true))
                                .ReturnsAsync(It.IsAny<List<Trainee>>());

            var result = await _traineeController.GetByPaymentStatusAsync(1, true);

            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<List<TraineeResponseDto>>(okObjectResult.Value);

            Assert.NotNull(model);
            Assert.IsType<List<TraineeResponseDto>>(model);
        }

        [Fact]
        public async void GetByPocketIdAsync_WhenTraineeExist_ShouldReturn_OkObjectResult_With_Resource()
        {
            _mockTraineeService.Setup(e => e.GetByPocketIdAsync(1, 1))
                                .ReturnsAsync(It.IsAny<List<Trainee>>());

            var result = await _traineeController.GetByPocketIdAsync(1, 1);

            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<List<TraineeResponseDto>>(okObjectResult.Value);

            Assert.NotNull(model);
            Assert.IsType<List<TraineeResponseDto>>(model);
        }

        [Fact]
        public async void GetByPersonIdAsync_WhenTraineeExist_ShouldReturn_OkObjectResult_With_Resource()
        {
            var trainee = CreateTrainee();

            _mockTraineeService.Setup(e => e.GetByPersonIdAsync(1, "1"))
                                .ReturnsAsync(trainee);

            var result = await _traineeController.GetByPersonIdAsync(1, "1");

            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<TraineeResponseDto>(okObjectResult.Value);

            Assert.NotNull(model);
            Assert.IsType<TraineeResponseDto>(model);
        }

        [Fact]
        public async void CreateAsync_WhenTraineeNotExist_ShouldReturn_OkObjectResult_With_Resource()
        {
            var trainee = CreateTrainee();

            _mockTraineeService.Setup(e => e.AddAsync(It.IsAny<Trainee>()))
                                .ReturnsAsync(trainee);

            var result = await _traineeController.CreateAsync(It.IsAny<TraineeCreateDto>());

            var createdAtActionResult = Assert.IsAssignableFrom<CreatedAtActionResult>(result);
            var model = Assert.IsAssignableFrom<TraineeResponseDto>(createdAtActionResult.Value);

            Assert.NotNull(model);
            Assert.IsType<TraineeResponseDto>(model);
        }

        [Fact]
        public async void CreateAsync_WhenTraineeAlreadyExist_ShouldReturn_ConflictIActionResult()
        {
            _mockTraineeService.Setup(e => e.AddAsync(It.IsAny<Trainee>()))
                                .Throws<ResourceExistException>();

            var result = await _traineeController.CreateAsync(It.IsAny<TraineeCreateDto>());

            var actionResult = (IStatusCodeActionResult)result;

            Assert.Equal(409, actionResult.StatusCode);
        }

        [Fact]
        public async void UpdateAsync_WhenTraineeExist_ShouldUpdateAndReturn_OkObjectResult_With_Resource()
        {
            var trainee = CreateTrainee();

            _mockTraineeService.Setup(e => e.UpdateAsync(1, It.IsAny<Trainee>()))
                                .ReturnsAsync(trainee);

            var result = await _traineeController.UpdateAsync(1, It.IsAny<TraineeUpdateDto>());

            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<TraineeResponseDto>(okObjectResult.Value);

            Assert.NotNull(model);
            Assert.IsType<TraineeResponseDto>(model);
        }

        [Fact]
        public async void UpdateAsync_WhenTraineeNotExist_ShouldReturn_NotFoundIActionResult()
        {
            _mockTraineeService.Setup(e => e.UpdateAsync(1, It.IsAny<Trainee>()))
                                .Throws<NotFoundException>();

            var result = await _traineeController.UpdateAsync(1, It.IsAny<TraineeUpdateDto>());

            var actionResult = (IStatusCodeActionResult)result;

            Assert.Equal(404, actionResult.StatusCode);
        }

        [Fact]
        public async void DeleteAsync_WhenTraineeExist_ShouldReturn_OkObjectResult_With_Resource()
        {
            var trainee = CreateTrainee();

            _mockTraineeService.Setup(e => e.DeleteAsync(1))
                                .ReturnsAsync(trainee);

            var result = await _traineeController.DeleteAsync(1);

            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<TraineeResponseDto>(okObjectResult.Value);

            Assert.NotNull(model);
            Assert.IsType<TraineeResponseDto>(model);
        }

        [Fact]
        public async void DeleteAsync_WhenSportTypeNotExist_ShouldReturn_NotFoundIActionResult()
        {
            _mockTraineeService.Setup(e => e.DeleteAsync(1))
                                .Throws<NotFoundException>();

            var result = await _traineeController.DeleteAsync(1);

            var actionResult = (IStatusCodeActionResult)result;

            Assert.Equal(404, actionResult.StatusCode);
        }

        private static Trainee CreateTrainee()
        {
            return new Trainee
            {
                Id = 1,
                ComplexId = 1,
                FirstName = "A",
                GroupId = 1,
                PersonId = "1",
                PocketId = 1,
                IsPaid = true
            };
        }

    }
}
