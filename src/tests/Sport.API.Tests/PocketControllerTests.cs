using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using Sport.API.Controllers;
using Sport.API.DTOs.PocketDto;
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
    public class PocketControllerTests
    {
        private readonly Mock<IPocketService> _mockPocketService;
        private readonly PocketController _pocketController;
        private readonly IMapper _mockMapper;

        public PocketControllerTests()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ResourceToModelProfile());
                cfg.AddProfile(new ModelToResourceProfile());
            });

            _mockMapper = mapperConfig.CreateMapper();
            _mockPocketService = new Mock<IPocketService>();
            _pocketController = new PocketController(_mockPocketService.Object, _mockMapper, null);
        }

        [Fact]
        public async void GetAllAsync_WhenPocketsExist_ShouldReturn_OkObjectResult_With_Resource()
        {
            var pockets = CreatePockets();

            _mockPocketService.Setup(e => e.GetAllAsync())
                            .ReturnsAsync(pockets);

            var result = await _pocketController.GetAllAsync();

            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<List<PocketResponseDto>>(okObjectResult.Value);

            Assert.NotNull(model);
            Assert.IsType<List<PocketResponseDto>>(model);
        }

        [Fact]
        public async void CreateAsync_WhenPocketNotExist_ShouldReturn_OkObjectResult_With_Resource()
        {
            var pocketCreateDto = new PocketCreateDto { Pocket = "AA", PricePerMonth = 1 };

            var pocket = CreatePocket();

            _mockPocketService.Setup(e => e.AddAsync(It.IsAny<Pocket>()))
                                .ReturnsAsync(pocket);

            var result = await _pocketController.CreateAsync(pocketCreateDto);

            var createdAtActionResult = Assert.IsAssignableFrom<CreatedAtActionResult>(result);
            var model = Assert.IsAssignableFrom<PocketResponseDto>(createdAtActionResult.Value);

            Assert.NotNull(model);
            Assert.IsType<PocketResponseDto>(model);
        }

        [Fact]
        public async void CreateAsync_WhenPocketExist_ShouldReturn_ConflictIActionResult()
        {
            _mockPocketService.Setup(e => e.AddAsync(It.IsAny<Pocket>()))
                                .Throws<ResourceExistException>();

            var result = await _pocketController.CreateAsync(It.IsAny<PocketCreateDto>());

            var actionResult = (IStatusCodeActionResult)result;

            Assert.Equal(409, actionResult.StatusCode);
        }

        [Fact]
        public async void UpdateAsync_WhenPocketExist_ShouldUpdateAndReturn_OkObjectResult_With_Resource()
        {
            var pocketUpdateDto = new PocketUpdateDto { PricePerMonth = 1 };

            var pocket = CreatePocket();

            _mockPocketService.Setup(e => e.UpdateAsync(1, It.IsAny<Pocket>()))
                                .ReturnsAsync(pocket);

            var result = await _pocketController.UpdateAsync(1, pocketUpdateDto);

            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<PocketResponseDto>(okObjectResult.Value);

            Assert.NotNull(model);
            Assert.IsType<PocketResponseDto>(model);
        }

        [Fact]
        public async void UpdateAsync_WhenPocketNotExist_ShouldReturn_NotFoundIActionResult()
        {
            _mockPocketService.Setup(e => e.UpdateAsync(1, It.IsAny<Pocket>()))
                                .Throws<NotFoundException>();

            var result = await _pocketController.UpdateAsync(1, It.IsAny<PocketUpdateDto>());

            var actionResult = (IStatusCodeActionResult)result;

            Assert.Equal(404, actionResult.StatusCode);
        }

        [Fact]
        public async void DeleteAsync_WhenPocketExist_ShouldReturn_OkObjectResult_With_Resource()
        {
            var pocket = CreatePocket();

            _mockPocketService.Setup(e => e.DeleteAsync(1))
                                .ReturnsAsync(pocket);

            var result = await _pocketController.DeleteAsync(1);

            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<PocketResponseDto>(okObjectResult.Value);

            Assert.NotNull(model);
            Assert.IsType<PocketResponseDto>(model);
        }

        [Fact]
        public async void DeleteAsync_WhenPocketNotExist_ShouldReturn_NotFoundiActionResult()
        {
            _mockPocketService.Setup(e => e.DeleteAsync(1))
                                .Throws<NotFoundException>();

            var result = await _pocketController.DeleteAsync(1);

            var actionResult = (IStatusCodeActionResult)result;

            Assert.Equal(404, actionResult.StatusCode);
        }

        private static IEnumerable<Pocket> CreatePockets()
        {
            return new List<Pocket>
            {
                new Pocket
                {
                    Id = 1,
                    Name = "AA",
                    PricePerMonth = 10
                },
                new Pocket
                {
                    Id = 2,
                    Name = "BB",
                    PricePerMonth = 20
                }
            };
        }

        private static Pocket CreatePocket()
        {
            return new Pocket
            {
                Id = 1,
                Name = "AA",
                PricePerMonth = 10
            };
        }
    }
}
