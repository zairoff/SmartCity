using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using Sport.API.Controllers;
using Sport.API.DTOs.EmployeeDto;
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
    public class EmployeeControllerTests
    {
        private readonly Mock<IEmployeeService> _mockEmployeeService;
        private readonly EmployeeController _employeeController;
        private readonly IMapper _mockMapper;

        public EmployeeControllerTests()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ResourceToModelProfile());
                cfg.AddProfile(new ModelToResourceProfile());
            });

            _mockMapper = mapperConfig.CreateMapper();
            _mockEmployeeService = new Mock<IEmployeeService>();
            _employeeController = new EmployeeController(_mockEmployeeService.Object, _mockMapper, null);
        }

        [Fact]
        public async void GetAllAsync_WhenEmployeeExist_ShouldReturn_OkObjectResult_With_Resource()
        {
            var employees = CreateEmployees();

            _mockEmployeeService.Setup(e => e.GetAllAsync())
                            .Returns(Task.FromResult(employees));

            var result = await _employeeController.GetAsync();

            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<List<EmployeeResponseDto>>(okObjectResult.Value);
            
            Assert.NotNull(model);
            Assert.IsType<List<EmployeeResponseDto>>(model);
            Assert.Equal("1", model[0].PersonId);
        }

        [Fact]
        public async void GetAsync_WhenEmployeeExist_ShouldReturn_OkObjectResult_With_Resource()
        {
            var employee = CreateEmployee();

            _mockEmployeeService.Setup(e => e.GetAsync(1))
                            .Returns(Task.FromResult(employee));

            var result = await _employeeController.GetAsync(1);

            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<EmployeeResponseDto>(okObjectResult.Value);

            Assert.NotNull(model);
            Assert.IsType<EmployeeResponseDto>(model);
            Assert.Equal("1", model.PersonId);
        }

        [Fact]
        public async void GetAsync_WhenEmployeeNotExist_ShouldReturn_ShouldReturn_NotFoundResponse()
        {
            _mockEmployeeService.Setup(e => e.GetAsync(1))
                            .Returns(Task.FromResult((Employee)null));

            var result = await _employeeController.GetAsync(1);

            var actionResult = (IStatusCodeActionResult)result;

            Assert.Equal(404, actionResult.StatusCode);
        }

        [Fact]
        public async Task CreateAsync_WhenEmployeeNotExist_ShouldReturn_OkObjectResult_With_Resource()
        {
            var employeeCreateDto = new EmployeeCreateDto
            {
                ComplexId = 1,
                PersonId = "1",
                PositionId = 1
            };

            var employee = _mockMapper.Map<EmployeeCreateDto, Employee>(employeeCreateDto);

            _mockEmployeeService.Setup(e => e.AddAsync(It.IsAny<Employee>()))
                                .ReturnsAsync(employee);

            var result = await _employeeController.CreateAsync(It.IsAny<EmployeeCreateDto>());

            var createdAtActionResult = Assert.IsAssignableFrom<CreatedAtActionResult>(result);
            var model = Assert.IsAssignableFrom<EmployeeResponseDto>(createdAtActionResult.Value);

            Assert.NotNull(model);
            Assert.IsType<EmployeeResponseDto>(model);
            Assert.Equal("1", model.PersonId);
        }

        [Fact]
        public async Task UpdateAsync_WhenEmployeeExist_Should_UpdateEmployeeAndReturn_OkObjectResult_With_Resource()
        {
            var employeeUpdate = new EmployeeUpdateDto { PositionId = "2" };

            var employee = CreateEmployee();

            _mockEmployeeService.Setup(e => e.UpdateAsync(1, It.IsAny<Employee>()))
                                .ReturnsAsync(employee);

            var result = await _employeeController.UpdateAsync(1, employeeUpdate);

            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<EmployeeResponseDto>(okObjectResult.Value);

            Assert.NotNull(model);
            Assert.IsType<EmployeeResponseDto>(model);
            Assert.Equal("1", model.PersonId);
        }

        [Fact]
        public async Task UpdateAsync_WhenEmployeeNotExist_ShouldReturn_NotFoundResponse()
        {
            var employeeUpdate = new EmployeeUpdateDto { PositionId = "2" };

            _mockEmployeeService.Setup(e => e.UpdateAsync(1, It.IsAny<Employee>()))
                                .Throws<NotFoundException>();

            var result = await _employeeController.UpdateAsync(1, employeeUpdate);

            var actionResult = (IStatusCodeActionResult)result;

            Assert.Equal(404, actionResult.StatusCode);
        }

        [Fact]
        public async Task DeleteAsync_WhenEmployeeExist_Should_DeleteEmployeeAndReturn_OkObjectResult_With_Resource()
        {
            var employee = CreateEmployee();

            _mockEmployeeService.Setup(e => e.DeleteAsync(1))
                                .ReturnsAsync(employee);

            var result = await _employeeController.DeleteAsync(1);

            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<EmployeeResponseDto>(okObjectResult.Value);

            Assert.NotNull(model);
            Assert.IsType<EmployeeResponseDto>(model);
            Assert.Equal("1", model.PersonId);
        }

        [Fact]
        public async Task DeleteAsync_WhenEmployeeNotExist_ShouldReturn_NotFoundResponse()
        {
            var employee = CreateEmployee();

            _mockEmployeeService.Setup(e => e.DeleteAsync(1))
                                .Throws<NotFoundException>();

            var result = await _employeeController.DeleteAsync(1);

            var actionResult = (IStatusCodeActionResult)result;

            Assert.Equal(404, actionResult.StatusCode);
        }

        private static IEnumerable<Employee> CreateEmployees()
        {
            return new List<Employee>
            {
                new Employee
                {
                    Id = 1,
                    ComplexId = 1,
                    PersonId = "1",
                    PositionId = 1
                },
                new Employee
                {
                    Id = 2,
                    ComplexId = 2,
                    PersonId = "2",
                    PositionId = 2
                }
            };
        }

        private static Employee CreateEmployee()
        {
            return new Employee
            {
                Id = 1,
                ComplexId = 1,
                PersonId = "1",
                PositionId = 1
            };
        }
    }
}
