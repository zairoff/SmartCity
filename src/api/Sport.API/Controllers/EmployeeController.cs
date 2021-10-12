using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sport.API.DTOs.EmployeeDto;
using Sport.Domain.Models;
using Sport.Service.Exceptions;
using Sport.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sport.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public EmployeeController(IEmployeeService employeeService, IMapper mapper, ILogger logger)
        {
            _employeeService = employeeService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Gets all employees
        /// </summary>
        /// <returns>List of employees</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync()
        {
            var employee = await _employeeService.GetAllAsync();
            var resource = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeResponseDto>>(employee);

            return Ok(resource);
        }

        /// <summary>
        /// Gets an employee
        /// </summary>
        /// <param name="id">id of employee</param>
        /// <returns>An EmployeeDto object</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync(int id)
        {
            var employee = await _employeeService.GetAsync(id);

            if (employee == null)
                return NotFound();

            var resource = _mapper.Map<Employee, EmployeeResponseDto>(employee);

            return Ok(resource);
        }

        /// <summary>
        /// Gets all employees
        /// </summary>
        /// <param name="complexId">ID of Complex</param>
        /// <returns>List of employees</returns>
        [HttpGet]
        [Route("GetByComplexId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByComplexIdAsync(int complexId)
        {
            var employee = await _employeeService.GetByComplexIdAsync(complexId);
            var resource = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeResponseDto>>(employee);

            return Ok(resource);
        }

        /// <summary>
        /// Gets an employee by personId
        /// </summary>
        /// <param name="complexId">ID of Complex</param>
        /// <param name="personId">ID of Person</param>
        /// <returns>An EmployeeDto object</returns>
        [HttpGet]
        [Route("GetByPersonId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByPersonIdAsync(int complexId, string personId)
        {
            var employee = await _employeeService.GetByPersonIdAsync(complexId, personId);

            if (employee == null)
                return NotFound();

            var resource = _mapper.Map<Employee, EmployeeResponseDto>(employee);

            return Ok(resource);
        }

        /// <summary>
        /// Gets list of employees by positionId
        /// </summary>
        /// <param name="complexId">ID of Complex</param>
        /// <param name="positionId">ID of position</param>
        /// <returns>List of EmployeeDto objects</returns>
        [HttpGet]
        [Route("GetByPositionId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByPositionIdAsync(int complexId, int positionId)
        {
            var employees = await _employeeService.GetByPositionIdAsync(complexId, positionId);

            var resource = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeResponseDto>>(employees);

            return Ok(resource);
        }

        /// <summary>
        /// Creates and saves an employee
        /// </summary>
        /// <param name="employeeCreateDto">EmployeeCreateDto object</param>
        /// <returns>Response for the request</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAsync([FromBody] EmployeeCreateDto employeeCreateDto)
        {
            try
            {
                var employee = _mapper.Map<EmployeeCreateDto, Employee>(employeeCreateDto);
                var result = await _employeeService.AddAsync(employee);

                var response = _mapper.Map<Employee, EmployeeResponseDto>(result);

                return CreatedAtAction(nameof(GetAsync), new { id = result.Id }, response);
            }
            catch (ResourceExistException ex)
            {
                return Conflict(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // to do: change hard coded string Exception to smth like enum
                await _logger.WriteAsync(
                                    ControllerContext.ActionDescriptor.ControllerName + ": " +
                                    ControllerContext.ActionDescriptor.ActionName +
                                    "Exception: " + ex.Message);

                return StatusCode(500);
            }
        }

        /// <summary>
        /// Updates existing employee
        /// </summary>
        /// <param name="id">id of Employee</param>
        /// <param name="employeeUpdateDto">EmployeeUpdateDto object</param>
        /// <returns>Response fot the request</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] EmployeeUpdateDto employeeUpdateDto)
        {
            try
            {
                var employee = _mapper.Map<EmployeeUpdateDto, Employee>(employeeUpdateDto);
                var result = await _employeeService.UpdateAsync(id, employee);

                var response = _mapper.Map<Employee, EmployeeResponseDto>(result);

                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // to do: change hard coded string Exception to smth like enum
                await _logger.WriteAsync(
                                    ControllerContext.ActionDescriptor.ControllerName + ": " +
                                    ControllerContext.ActionDescriptor.ActionName +
                                    "Exception: " + ex.Message);

                return StatusCode(500);
            }
        }

        /// <summary>
        /// Deletes existing employee
        /// </summary>
        /// <param name="id">id of employee</param>
        /// <returns>Response fot the request</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var result = await _employeeService.DeleteAsync(id);

                var response = _mapper.Map<Employee, EmployeeResponseDto>(result);

                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // to do: change hard coded string Exception to smth like enum
                await _logger.WriteAsync(
                                    ControllerContext.ActionDescriptor.ControllerName + ": " +
                                    ControllerContext.ActionDescriptor.ActionName +
                                    "Exception: " + ex.Message);

                return StatusCode(500);
            }
        }
    }
}
