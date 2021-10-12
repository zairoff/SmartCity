using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using Sport.API.Controllers;
using Sport.API.DTOs.TrainerGroupDto;
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
    public class TrainerGroupControllerTests
    {
        private readonly Mock<ITrainerGroupService> _mockTrainerGroupService;
        private readonly TrainerGroupController _trainerGroupController;
        private readonly IMapper _mockMapper;

        public TrainerGroupControllerTests()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ResourceToModelProfile());
                cfg.AddProfile(new ModelToResourceProfile());
            });

            _mockMapper = mapperConfig.CreateMapper();
            _mockTrainerGroupService = new Mock<ITrainerGroupService>();
            _trainerGroupController = new TrainerGroupController(_mockTrainerGroupService.Object, _mockMapper, null);
        }

        [Fact]
        public async void GetAsync_WhenTrainerGroupExist_ShouldReturn_OkObjectResult_With_Resource()
        {
            _mockTrainerGroupService.Setup(e => e.GetAllAsync())
                                .ReturnsAsync(It.IsAny<List<TrainerGroup>>());

            var result = await _trainerGroupController.GetAsync();

            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<List<TrainerGroupResponseDto>>(okObjectResult.Value);

            Assert.NotNull(model);
            Assert.IsType<List<TrainerGroupResponseDto>>(model);
        }

        [Fact]
        public async void GetByGroupIdAsync_WhenTrainerGroupExist_ShouldReturn_OkObjectResult_With_Resource()
        {
            _mockTrainerGroupService.Setup(e => e.GetByGroupIdAsync(1))
                                .ReturnsAsync(It.IsAny<List<TrainerGroup>>());

            var result = await _trainerGroupController.GetByGroupIdAsync(1);

            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<List<TrainerGroupResponseDto>>(okObjectResult.Value);

            Assert.NotNull(model);
            Assert.IsType<List<TrainerGroupResponseDto>>(model);
        }

        [Fact]
        public async void GetByTrainerIdAsync_WhenTrainerGroupExist_ShouldReturn_OkObjectResult_With_Resource()
        {
            _mockTrainerGroupService.Setup(e => e.GetByTrainerIdAsync(1))
                                .ReturnsAsync(It.IsAny<List<TrainerGroup>>());

            var result = await _trainerGroupController.GetByTrainerIdAsync(1);

            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<List<TrainerGroupResponseDto>>(okObjectResult.Value);

            Assert.NotNull(model);
            Assert.IsType<List<TrainerGroupResponseDto>>(model);
        }

        [Fact]
        public async void CreateAsync_WhenTrainerGroupNotExist_ShouldReturn_OkObjectResult_With_Resource()
        {
            var trainerGroup = CreateTrainerGroup();

            _mockTrainerGroupService.Setup(e => e.AddAsync(It.IsAny<TrainerGroup>()))
                                .ReturnsAsync(trainerGroup);

            var result = await _trainerGroupController.CreateAsync(It.IsAny<TrainerGroupCreateDto>());

            var createdAtActionResult = Assert.IsAssignableFrom<CreatedAtActionResult>(result);
            var model = Assert.IsAssignableFrom<TrainerGroupResponseDto>(createdAtActionResult.Value);

            Assert.NotNull(model);
            Assert.IsType<TrainerGroupResponseDto>(model);
        }

        [Fact]
        public async void CreateAsync_WhenTrainerGroupAlreadyExist_ShouldReturn_ConflictIActionResult()
        {
            _mockTrainerGroupService.Setup(e => e.AddAsync(It.IsAny<TrainerGroup>()))
                                .Throws<ResourceExistException>();

            var result = await _trainerGroupController.CreateAsync(It.IsAny<TrainerGroupCreateDto>());

            var actionResult = (IStatusCodeActionResult)result;

            Assert.Equal(409, actionResult.StatusCode);
        }

        [Fact]
        public async void DeleteAsync_WhenTrainerGroupExist_ShouldReturn_OkObjectResult_With_Resource()
        {
            var trainer = CreateTrainerGroup();

            _mockTrainerGroupService.Setup(e => e.DeleteAsync(1))
                                .ReturnsAsync(trainer);

            var result = await _trainerGroupController.DeleteAsync(1);

            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<TrainerGroupResponseDto>(okObjectResult.Value);

            Assert.NotNull(model);
            Assert.IsType<TrainerGroupResponseDto>(model);
        }

        [Fact]
        public async void DeleteAsync_WhenTrainerGroupNotExist_ShouldReturn_ConflictIActionResult()
        {
            _mockTrainerGroupService.Setup(e => e.DeleteAsync(1))
                                .Throws<NotFoundException>();

            var result = await _trainerGroupController.DeleteAsync(1);

            var actionResult = (IStatusCodeActionResult)result;

            Assert.Equal(404, actionResult.StatusCode);
        }

        private static TrainerGroup CreateTrainerGroup()
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
