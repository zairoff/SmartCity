using Moq;
using Sport.Domain.Models;
using Sport.Infrastructure.Base;
using Sport.Service.Interfaces;
using System.Collections.Generic;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using MockQueryable.Moq;
using Sport.Service.Tests.Extensions;
using Sport.Service.Exceptions;
using System.Threading.Tasks;

namespace Sport.Service.Tests
{
    public class EmployeeServiceTests
    {
        private readonly Mock<IRepository<Employee>> _mockEmployeeRepository;
        private readonly IEmployeeService _employeeService;
        private readonly Mock<IQueryable<Employee>> _mockEmployeeList;
        private readonly Mock<IQueryable<Employee>> _mockEmptyEmployeeList;
        private readonly Mock<IQueryable<Employee>> _mockEmployee;

        public EmployeeServiceTests()
        {
            _mockEmployeeRepository = new Mock<IRepository<Employee>>();
            _employeeService = new EmployeeService(_mockEmployeeRepository.Object);
            _mockEmployeeList = CreateEmployeeList().AsQueryable().BuildMock();
            _mockEmptyEmployeeList = Enumerable.Empty<Employee>().AsQueryable().BuildMock();
            _mockEmployee = CreateEmployee().ToQueryable().BuildMock();
        }

        [Fact]
        public async void GetAllAsync_WhenEmployeeExist_ShouldReturnListOfEmployees()
        {
            _mockEmployeeRepository.Setup(e => e.GetAll()).Returns(_mockEmployeeList.Object);

            var result = await _employeeService.GetAllAsync();

            Assert.NotNull(result);
            Assert.True(result.Count() == 2);
            Assert.True(result.FirstOrDefault().Id == 1);
            Assert.IsType<List<Employee>>(result);
        }

        [Fact]
        public async void GetAllAsync_WhenEmployeeNotExist_ShouldReturnEmptyList()
        {
            _mockEmployeeRepository.Setup(e => e.GetAll())
                                    .Returns(_mockEmptyEmployeeList.Object);

            var result = await _employeeService.GetAllAsync();

            Assert.True(result.Count() == 0);
        }

        [Fact]
        public async void GetByPersonIdAsync_WhenEmployeeExist_ShouldReturnEmployee()
        {          
            _mockEmployeeRepository.Setup(e => e.Get()).Returns(_mockEmployee.Object);

            var result = await _employeeService.GetByPersonIdAsync(1, "1");

            Assert.NotNull(result);
            Assert.True(result.Id == 1);
            Assert.True(result.PersonId == "1");
            Assert.True(result.ComplexId == 1);
            Assert.IsType<Employee>(result);
        }

        [Fact]
        public async void GetByPersonIdAsync_WhenEmployeeNotExist_ShouldReturnNull()
        {
            _mockEmployeeRepository.Setup(e => e.Get()).Returns(_mockEmptyEmployeeList.Object);

            var result = await _employeeService.GetByPersonIdAsync(1, "1");

            Assert.Null(result);
        }

        [Fact]
        public async void AddAsync_WhenEmployeeNotExist_ShouldAddEmployee()
        {
            _mockEmployeeRepository.Setup(e => e.Get()).Returns(_mockEmptyEmployeeList.Object);
            _mockEmployeeRepository.Setup(e => e.AddAsync(CreateEmployee()));                                    

            var result = await _employeeService.AddAsync(CreateEmployee());

            Assert.NotNull(result);
            Assert.IsType<Employee>(result);
        }

        [Fact]
        public async void AddAsync_WhenEmployeeAlreadyExist_ShouldThrowException()
        {
            _mockEmployeeRepository.Setup(e => e.Get()).Returns(_mockEmployee.Object);

            await Assert.ThrowsAsync<ResourceExistException>(() => _employeeService.AddAsync(CreateEmployee()));
        }

        [Fact]
        public async void AddAsync_ShouldCallAddFromRepository_OnlyOnce()
        {
            var employee = CreateEmployee();

            _mockEmployeeRepository.Setup(e => e.Get()).Returns(_mockEmptyEmployeeList.Object);
            _mockEmployeeRepository.Setup(e => e.AddAsync(employee));

            await _employeeService.AddAsync(employee);

            _mockEmployeeRepository.Verify(mock => mock.AddAsync(employee), Times.Once);
        }

        [Fact]
        public async void UpdateAsync_WhenEmployeeNotExist_ShouldThrowException()
        {
            _mockEmployeeRepository.Setup(e => e.Get()).Returns(_mockEmptyEmployeeList.Object);

            await Assert.ThrowsAsync<NotFoundException>(
                        () => _employeeService.UpdateAsync(1, CreateEmployee()));

        }

        [Fact]
        public async void UpdateAsync_WhenEmployeeExist_ShouldReturnEmployee()
        {
            var employee = CreateEmployee();

            _mockEmployeeRepository.Setup(e => e.Get()).Returns(_mockEmployee.Object);
            _mockEmployeeRepository.Setup(e => e.UpdateAsync(employee));

            var result = await _employeeService.UpdateAsync(1, employee);

            Assert.NotNull(result);
            Assert.IsType<Employee>(result);
        }

        [Fact]
        public async void UpdateAsync_ShouldCallAddFromRepository_OnlyOnce()
        {
            var employee = CreateEmployee();
            var mock = employee.ToQueryable().BuildMock();

            _mockEmployeeRepository.Setup(e => e.Get()).Returns(mock.Object);
            _mockEmployeeRepository.Setup(e => e.UpdateAsync(employee));

            await _employeeService.UpdateAsync(1, employee);

            _mockEmployeeRepository.Verify(mock => mock.UpdateAsync(employee), Times.Once);
        }

        [Fact]
        public async void DeleteAsync_WhenEmployeeExist_ShouldReturnEmployee()
        {
            var employee = CreateEmployee();

            _mockEmployeeRepository.Setup(e => e.Get()).Returns(_mockEmployee.Object);
            _mockEmployeeRepository.Setup(e => e.DeleteAsync(employee));

            var result = await _employeeService.DeleteAsync(1);

            Assert.NotNull(result);
            Assert.IsType<Employee>(result);
        }

        private static Employee CreateEmployee()
        {
            return new Employee()
            {
                Id = 1,
                ComplexId = 1,
                FirstName = "John",
                LastName = "Smith",
                PersonId = "1",
                PositionId = 1
            };
        }

        private static IEnumerable<Employee> CreateEmployeeList()
        {
            return new List<Employee>()
            {
                new Employee()
                {
                    Id = 1,
                    ComplexId = 1,
                    FirstName = "John",
                    LastName = "Smith",
                    PersonId = "1",
                    PositionId = 1
                },
                new Employee()
                {
                    Id = 2,
                    ComplexId = 2,
                    FirstName = "John",
                    LastName = "Smith",
                    PersonId = "2",
                    PositionId = 2
                }
            };
        }
    }
}
