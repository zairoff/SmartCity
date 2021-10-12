using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using Sport.API.Controllers;
using Sport.API.DTOs.SportTypeDto;
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
    public class SportTypeControllerTests
    {
        private readonly Mock<ISportTypeService> _mockSportTypeService;
        private readonly SportTypeController _sportTypeController;
        private readonly IMapper _mockMapper;
        
        public SportTypeControllerTests()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ResourceToModelProfile());
                cfg.AddProfile(new ModelToResourceProfile());
            });

            _mockMapper = mapperConfig.CreateMapper();
            _mockSportTypeService = new Mock<ISportTypeService>();
            _sportTypeController = new SportTypeController(_mockSportTypeService.Object, _mockMapper, null);
        }

        [Fact]
        public async void GetAllAsync_WhenSportTypeExist_ShouldReturn_OkObjectResult_With_Resource()
        {
            var sports = CreateSports();

            _mockSportTypeService.Setup(e => e.GetAllAsync())
                            .ReturnsAsync(sports);

            var result = await _sportTypeController.GetAsync();

            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<List<SportTypeResponseDto>>(okObjectResult.Value);

            Assert.NotNull(model);
            Assert.IsType<List<SportTypeResponseDto>>(model);
        }

        [Fact]
        public async void CreateAsync_WhenSportTypeNotExist_ShouldReturn_OkObjectResult_With_Resource()
        {
            var sportType = CreateSport();

            _mockSportTypeService.Setup(e => e.AddAsync(It.IsAny<SportType>()))
                                .ReturnsAsync(sportType);

            var result = await _sportTypeController.CreateAsync(It.IsAny<SportTypeCreateDto>());

            var createdAtActionResult = Assert.IsAssignableFrom<CreatedAtActionResult>(result);
            var model = Assert.IsAssignableFrom<SportTypeResponseDto>(createdAtActionResult.Value);

            Assert.NotNull(model);
            Assert.IsType<SportTypeResponseDto>(model);
        }

        [Fact]
        public async void CreateAsync_WhenSportTypeAlreadyExist_ShouldReturn_ConflictIActionResult()
        {
            _mockSportTypeService.Setup(e => e.AddAsync(It.IsAny<SportType>()))
                                .Throws<ResourceExistException>();

            var result = await _sportTypeController.CreateAsync(It.IsAny<SportTypeCreateDto>());

            var actionResult = (IStatusCodeActionResult)result;

            Assert.Equal(409, actionResult.StatusCode);
        }

        [Fact]
        public async void UpdateAsync_WhenSportTypeExist_ShouldUpdateAndReturn_OkObjectResult_With_Resource()
        {
            var sportType = CreateSport();

            _mockSportTypeService.Setup(e => e.UpdateAsync(1, It.IsAny<SportType>()))
                                .ReturnsAsync(sportType);

            var result = await _sportTypeController.UpdateAsync(1, It.IsAny<SportTypeUpdateDto>());

            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<SportTypeResponseDto>(okObjectResult.Value);

            Assert.NotNull(model);
            Assert.IsType<SportTypeResponseDto>(model);
        }

        [Fact]
        public async void UpdateAsync_WhenSportTypeNotExist_ShouldReturn_NotFoundIActionResult()
        {
            _mockSportTypeService.Setup(e => e.UpdateAsync(1, It.IsAny<SportType>()))
                                .Throws<NotFoundException>();

            var result = await _sportTypeController.UpdateAsync(1, It.IsAny<SportTypeUpdateDto>());

            var actionResult = (IStatusCodeActionResult)result;

            Assert.Equal(404, actionResult.StatusCode);
        }

        [Fact]
        public async void DeleteAsync_WhenSportTypeExist_ShouldReturn_OkObjectResult_With_Resource()
        {
            var sportType = CreateSport();

            _mockSportTypeService.Setup(e => e.DeleteAsync(1))
                                .ReturnsAsync(sportType);

            var result = await _sportTypeController.DeleteAsync(1);

            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<SportTypeResponseDto>(okObjectResult.Value);

            Assert.NotNull(model);
            Assert.IsType<SportTypeResponseDto>(model);
        }

        [Fact]
        public async void DeleteAsync_WhenSportTypeNotExist_ShouldReturn_NotFoundiActionResult()
        {
            _mockSportTypeService.Setup(e => e.DeleteAsync(1))
                                .Throws<NotFoundException>();

            var result = await _sportTypeController.DeleteAsync(1);

            var actionResult = (IStatusCodeActionResult)result;

            Assert.Equal(404, actionResult.StatusCode);
        }

        private static IEnumerable<SportType> CreateSports()
        {
            return new List<SportType>
            {
                new SportType
                {
                    Id = 1,
                    Name = "AA"
                },
                new SportType
                {
                    Id = 2,
                    Name = "BB"
                }
            };
        }

        private static SportType CreateSport()
        {
            return new SportType
            {
                Id = 1,
                Name = "AA"
            };
        }
    }
}
