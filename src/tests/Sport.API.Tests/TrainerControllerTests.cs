using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using Sport.API.Controllers;
using Sport.API.DTOs.TrainerDto;
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
    public class TrainerControllerTests
    {
        private readonly Mock<ITrainerService> _mockTrainerService;
        private readonly TrainerController _trainerController;
        private readonly IMapper _mockMapper;

        public TrainerControllerTests()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ResourceToModelProfile());
                cfg.AddProfile(new ModelToResourceProfile());
            });

            _mockMapper = mapperConfig.CreateMapper();
            _mockTrainerService = new Mock<ITrainerService>();
            _trainerController = new TrainerController(_mockTrainerService.Object, _mockMapper, null);
        }

        [Fact]
        public async void GetAllAsync_WhenTraineeExist_ShouldReturn_OkObjectResult_With_Resource()
        {
            _mockTrainerService.Setup(e => e.GetAllAsync())
                                .ReturnsAsync(It.IsAny<List<Trainer>>());

            var result = await _trainerController.GetAsync();

            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<List<TrainerResponseDto>>(okObjectResult.Value);

            Assert.NotNull(model);
            Assert.IsType<List<TrainerResponseDto>>(model);
        }

        [Fact]
        public async void GetBySportTypeIdAsync_WhenTrainerExist_ShouldReturn_OkObjectResult_With_Resource()
        {
            _mockTrainerService.Setup(e => e.GetBySportTypeIdAsync(1, 1))
                                .ReturnsAsync(It.IsAny<List<Trainer>>());

            var result = await _trainerController.GetBySportTypeIdAsync(1, 1);

            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<List<TrainerResponseDto>>(okObjectResult.Value);

            Assert.NotNull(model);
            Assert.IsType<List<TrainerResponseDto>>(model);
        }

        [Fact]
        public async void GetByEmployeeIdAsync_WhenTrainerExist_ShouldReturn_OkObjectResult_With_Resource()
        {
            var trainer = CreateTrainer();

            _mockTrainerService.Setup(e => e.GetByEmployeeIdAsync(1, 1))
                                .ReturnsAsync(trainer);

            var result = await _trainerController.GetByEmployeeIdAsync(1, 1);

            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<TrainerResponseDto>(okObjectResult.Value);

            Assert.NotNull(model);
            Assert.IsType<TrainerResponseDto>(model);
        }

        [Fact]
        public async void CreateAsync_WhenTrainerNotExist_ShouldReturn_OkObjectResult_With_Resource()
        {
            var trainer = CreateTrainer();

            _mockTrainerService.Setup(e => e.AddAsync(It.IsAny<Trainer>()))
                                .ReturnsAsync(trainer);

            var result = await _trainerController.CreateAsync(It.IsAny<TrainerCreateDto>());

            var createdAtActionResult = Assert.IsAssignableFrom<CreatedAtActionResult>(result);
            var model = Assert.IsAssignableFrom<TrainerResponseDto>(createdAtActionResult.Value);

            Assert.NotNull(model);
            Assert.IsType<TrainerResponseDto>(model);
        }

        [Fact]
        public async void CreateAsync_WhenTrainerAlreadyExist_ShouldReturn_ConflictIActionResult()
        {
            _mockTrainerService.Setup(e => e.AddAsync(It.IsAny<Trainer>()))
                                .Throws<ResourceExistException>();

            var result = await _trainerController.CreateAsync(It.IsAny<TrainerCreateDto>());

            var actionResult = (IStatusCodeActionResult)result;

            Assert.Equal(409, actionResult.StatusCode);
        }

        [Fact]
        public async void UpdateAsync_WhenTrainerExist_ShouldUpdateAndReturn_OkObjectResult_With_Resource()
        {
            var trainer = CreateTrainer();

            _mockTrainerService.Setup(e => e.UpdateAsync(1, It.IsAny<Trainer>()))
                                .ReturnsAsync(trainer);

            var result = await _trainerController.UpdateAsync(1, It.IsAny<TrainerUpdateDto>());

            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<TrainerResponseDto>(okObjectResult.Value);

            Assert.NotNull(model);
            Assert.IsType<TrainerResponseDto>(model);
        }

        [Fact]
        public async void UpdateAsync_WhenTrainerNotExist_ShouldReturn_ConflictIActionResult()
        {
            _mockTrainerService.Setup(e => e.UpdateAsync(1, It.IsAny<Trainer>()))
                                .Throws<NotFoundException>();

            var result = await _trainerController.UpdateAsync(1, It.IsAny<TrainerUpdateDto>());

            var actionResult = (IStatusCodeActionResult)result;

            Assert.Equal(404, actionResult.StatusCode);
        }

        [Fact]
        public async void DeleteAsync_WhenTrainerExist_ShouldReturn_OkObjectResult_With_Resource()
        {
            var trainer = CreateTrainer();

            _mockTrainerService.Setup(e => e.DeleteAsync(1))
                                .ReturnsAsync(trainer);

            var result = await _trainerController.DeleteAsync(1);

            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<TrainerResponseDto>(okObjectResult.Value);

            Assert.NotNull(model);
            Assert.IsType<TrainerResponseDto>(model);
        }

        [Fact]
        public async void DeleteAsync_WhenTrainerNotExist_ShouldReturn_ConflictIActionResult()
        {
            _mockTrainerService.Setup(e => e.UpdateAsync(1, It.IsAny<Trainer>()))
                                .Throws<NotFoundException>();

            var result = await _trainerController.UpdateAsync(1, It.IsAny<TrainerUpdateDto>());

            var actionResult = (IStatusCodeActionResult)result;

            Assert.Equal(404, actionResult.StatusCode);
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
