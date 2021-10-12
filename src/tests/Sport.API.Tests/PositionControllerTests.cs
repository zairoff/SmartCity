using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using Sport.API.Controllers;
using Sport.API.DTOs.PositionDto;
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
    public class PositionControllerTests
    {
        private readonly Mock<IPositionService> _mockPositionService;
        private readonly PositionController _positionController;
        private readonly IMapper _mockMapper;

        public PositionControllerTests()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ResourceToModelProfile());
                cfg.AddProfile(new ModelToResourceProfile());
            });

            _mockMapper = mapperConfig.CreateMapper();
            _mockPositionService = new Mock<IPositionService>();
            _positionController = new PositionController(_mockPositionService.Object, _mockMapper, null);
        }


        [Fact]
        public async void GetAsync_WhenPositionExist_ShouldReturn_OkObjectResult_With_Resource()
        {
            var positions = CreatePositions();

            _mockPositionService.Setup(e => e.GetAllAsync())
                        .ReturnsAsync(positions);

            var result = await _positionController.GetAsync();

            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<List<PositionResponseDto>>(okObjectResult.Value);
            
            Assert.IsType<List<PositionResponseDto>>(model);
        }

        [Fact]
        public async void CreateAsync_WhenPositionNotExist_ShouldReturn_OkObjectResult_With_Resource()
        {
            var position = CreatePosition();

            _mockPositionService.Setup(e => e.AddAsync(It.IsAny<Position>()))
                                .ReturnsAsync(position);

            var result = await _positionController
                                .CreateAsync(It.IsAny<PositionCreateDto>());

            var createdAtActionResult = Assert.IsAssignableFrom<CreatedAtActionResult>(result);
            var model = Assert.IsAssignableFrom<PositionResponseDto>(createdAtActionResult.Value);

            Assert.NotNull(model);
            Assert.IsType<PositionResponseDto>(model);
        }

        [Fact]
        public async void CreateAsync_WhenPositionExist_ShouldReturn_ConflictIActionResult()
        {
            var position = CreatePosition();

            _mockPositionService.Setup(e => e.AddAsync(It.IsAny<Position>()))
                                .Throws<ResourceExistException>();

            var result = await _positionController
                                .CreateAsync(It.IsAny<PositionCreateDto>());

            var actionResult = (IStatusCodeActionResult)result;

            Assert.Equal(409, actionResult.StatusCode);
        }

        [Fact]
        public async void UpdateAsync_WhenPositionExist_ShouldUpdateAndReturn_OkObjectResult_With_Resource()
        {
            var position = CreatePosition();

            _mockPositionService.Setup(e => e.UpdateAsync(1, It.IsAny<Position>()))
                                .ReturnsAsync(position);

            var result = await _positionController.UpdateAsync(1, It.IsAny<PositionUpdateDto>());

            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<PositionResponseDto>(okObjectResult.Value);

            Assert.NotNull(model);
            Assert.IsType<PositionResponseDto>(model);
        }

        [Fact]
        public async void UpdateAsync_WhenPositiontNotExist_ShouldReturn_NotFoundIActionResult()
        {
            _mockPositionService.Setup(e => e.UpdateAsync(1, It.IsAny<Position>()))
                                .Throws<NotFoundException>();

            var result = await _positionController.UpdateAsync(1, It.IsAny<PositionUpdateDto>());

            var actionResult = (IStatusCodeActionResult)result;

            Assert.Equal(404, actionResult.StatusCode);
        }

        [Fact]
        public async void DeleteAsync_WhenPositionExist_ShouldReturn_OkObjectResult_With_Resource()
        {
            var position = CreatePosition();

            _mockPositionService.Setup(e => e.DeleteAsync(1))
                                .ReturnsAsync(position);

            var result = await _positionController.DeleteAsync(1);

            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<PositionResponseDto>(okObjectResult.Value);

            Assert.NotNull(model);
            Assert.IsType<PositionResponseDto>(model);
        }

        [Fact]
        public async void DeleteAsync_WhenPositionNotExist_ShouldReturn_NotFoundiActionResult()
        {
            _mockPositionService.Setup(e => e.DeleteAsync(1))
                                .Throws<NotFoundException>();

            var result = await _positionController.DeleteAsync(1);

            var actionResult = (IStatusCodeActionResult)result;

            Assert.Equal(404, actionResult.StatusCode);
        }

        private static IEnumerable<Position> CreatePositions()
        {
            return new List<Position>
            {
                new Position
                {
                    Id = 1,
                    Name = "AA"
                },
                new Position
                {
                    Id = 2,
                    Name = "BB"
                }
            };
        }

        private static Position CreatePosition()
        {
            return new Position
            {
                Id = 1,
                Name = "AA"
            };
        }
    }
}
