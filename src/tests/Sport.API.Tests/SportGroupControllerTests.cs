using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using Sport.API.Controllers;
using Sport.API.DTOs.SportGroupDto;
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
    public class SportGroupControllerTests
    {
        private readonly Mock<ISportGroupService> _mockSportGroupService;
        private readonly SportGroupController _sportGroupController;
        private readonly IMapper _mockMapper;

        public SportGroupControllerTests()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ResourceToModelProfile());
                cfg.AddProfile(new ModelToResourceProfile());
            });

            _mockMapper = mapperConfig.CreateMapper();
            _mockSportGroupService = new Mock<ISportGroupService>();
            _sportGroupController = new SportGroupController(_mockSportGroupService.Object, _mockMapper, null);
        }

        [Fact]
        public async void GetAllAsync_WhenSportGroupsExist_ShouldReturn_OkObjectResult_With_Resource()
        {
            var groups = CreateGroups();

            _mockSportGroupService.Setup(e => e.GetAllAsync())
                            .ReturnsAsync(groups);

            var result = await _sportGroupController.GetAsync();

            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<List<SportGroupResponseDto>>(okObjectResult.Value);

            Assert.NotNull(model);
            Assert.IsType<List<SportGroupResponseDto>>(model);
        }

        [Fact]
        public async void CreateAsync_WhenSportGroupNotExist_ShouldReturn_OkObjectResult_With_Resource()
        {
            var group = CreateGroup();

            _mockSportGroupService.Setup(e => e.AddAsync(It.IsAny<SportGroup>()))
                                .ReturnsAsync(group);

            var result = await _sportGroupController.CreateAsync(It.IsAny<SportGroupCreateDto>());

            var createdAtActionResult = Assert.IsAssignableFrom<CreatedAtActionResult>(result);
            var model = Assert.IsAssignableFrom<SportGroupResponseDto>(createdAtActionResult.Value);

            Assert.NotNull(model);
            Assert.IsType<SportGroupResponseDto>(model);
        }

        [Fact]
        public async void CreateAsync_WhenSportGroupAlreadyExist_ShouldReturn_ConflictIActionResult()
        {
            _mockSportGroupService.Setup(e => e.AddAsync(It.IsAny<SportGroup>()))
                                .Throws<ResourceExistException>();

            var result = await _sportGroupController.CreateAsync(It.IsAny<SportGroupCreateDto>());

            var actionResult = (IStatusCodeActionResult)result;

            Assert.Equal(409, actionResult.StatusCode);
        }

        [Fact]
        public async void UpdateAsync_WhenSportGroupExist_ShouldUpdateAndReturn_OkObjectResult_With_Resource()
        {
            var pocket = CreateGroup();

            _mockSportGroupService.Setup(e => e.UpdateAsync(1, It.IsAny<SportGroup>()))
                                .ReturnsAsync(pocket);

            var result = await _sportGroupController.UpdateAsync(1, It.IsAny<SportGroupUpdateDto>());

            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<SportGroupResponseDto>(okObjectResult.Value);

            Assert.NotNull(model);
            Assert.IsType<SportGroupResponseDto>(model);
        }

        [Fact]
        public async void UpdateAsync_WhenSportGroupNotExist_ShouldReturn_NotFoundIActionResult()
        {
            _mockSportGroupService.Setup(e => e.UpdateAsync(1, It.IsAny<SportGroup>()))
                                .Throws<NotFoundException>();

            var result = await _sportGroupController.UpdateAsync(1, It.IsAny<SportGroupUpdateDto>());

            var actionResult = (IStatusCodeActionResult)result;

            Assert.Equal(404, actionResult.StatusCode);
        }

        [Fact]
        public async void DeleteAsync_WhenSportGroupExist_ShouldReturn_OkObjectResult_With_Resource()
        {
            var group = CreateGroup();

            _mockSportGroupService.Setup(e => e.DeleteAsync(1))
                                .ReturnsAsync(group);

            var result = await _sportGroupController.DeleteAsync(1);

            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<SportGroupResponseDto>(okObjectResult.Value);

            Assert.NotNull(model);
            Assert.IsType<SportGroupResponseDto>(model);
        }

        [Fact]
        public async void DeleteAsync_WhenSportGroupNotExist_ShouldReturn_NotFoundiActionResult()
        {
            _mockSportGroupService.Setup(e => e.DeleteAsync(1))
                                .Throws<NotFoundException>();

            var result = await _sportGroupController.DeleteAsync(1);

            var actionResult = (IStatusCodeActionResult)result;

            Assert.Equal(404, actionResult.StatusCode);
        }

        private static IEnumerable<SportGroup> CreateGroups()
        {
            return new List<SportGroup>
            {
                new SportGroup
                {
                    Id = 1,
                    Name = "AA",
                    SportTypeId = 1
                },
                new SportGroup
                {
                    Id = 2,
                    Name = "BB",
                    SportTypeId = 1
                }
            };
        }

        private static SportGroup CreateGroup()
        {
            return new SportGroup
            {
                Id = 1,
                Name = "AA",
                SportTypeId = 1
            };
        }
    }
}
