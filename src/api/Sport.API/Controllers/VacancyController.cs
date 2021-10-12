using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sport.API.DTOs.VacancyDto;
using Sport.Domain.Models;
using Sport.Service.Exceptions;
using Sport.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VacancyController : ControllerBase
    {
        private readonly IVacancyService _vacancyService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public VacancyController(IVacancyService vacancyService, IMapper mapper, ILogger logger)
        {
            _vacancyService = vacancyService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Gets all vacancies
        /// </summary>
        /// <returns>List of vacancies</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync()
        {
            var vacancies = await _vacancyService.GetAllAsync();

            var resource = _mapper.Map<IEnumerable<Vacancy>, IEnumerable<VacancyResponseDto>>(vacancies);

            return Ok(resource);
        }

        /// <summary>
        /// Gets all vacancies
        /// </summary>
        /// <param name="complexId">id of Complex</param>
        /// <returns>List of vacancies</returns>
        [HttpGet]
        [Route("GetByComplexId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByComplexIdAsync(int complexId)
        {
            var vacancies = await _vacancyService.GetByComplexIdAsync(complexId);

            var resource = _mapper.Map<IEnumerable<Vacancy>, IEnumerable<VacancyResponseDto>>(vacancies);

            return Ok(resource);
        }

        /// <summary>
        /// Gets a vacancy
        /// </summary>
        /// <param name="id">id of vacancy</param>
        /// <returns>Vacancy object</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync(int id)
        {
            var vacancy = await _vacancyService.GetAsync(id);

            if (vacancy == null)
                return NotFound();

            var resource = _mapper.Map<Vacancy, VacancyResponseDto>(vacancy);

            return Ok(resource);
        }

        /// <summary>
        /// Gets list of vacancies
        /// </summary>
        /// <param name="complexId">id of Complex</param>
        /// <param name="positionId">id of Position</param>
        /// <returns>List of vacancies</returns>
        [HttpGet]
        [Route("GetByPositionId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByPositionIdAsync(int complexId, int positionId)
        {
            var vacancies = await _vacancyService.GetByPositionIdAsync(complexId, positionId);

            var resource = _mapper.Map<IEnumerable<Vacancy>, IEnumerable<VacancyResponseDto>>(vacancies);

            return Ok(resource);
        }

        /// <summary>
        /// Gets list of vacancies
        /// </summary>
        /// <param name="complexId">status of Complex</param>
        /// <param name="isActive">status of vacancy</param>
        /// <returns>List of vacancies</returns>
        [HttpGet]
        [Route("GetByStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByStatusAsync(int complexId, bool isActive)
        {
            var vacancies = await _vacancyService.GetByStatusAsync(complexId, isActive);

            var resource = _mapper.Map<IEnumerable<Vacancy>, IEnumerable<VacancyResponseDto>>(vacancies);

            return Ok(resource);
        }

        /// <summary>
        /// Creates vacancy
        /// </summary>
        /// <param name="vacancyCreateDto">vacancyCreateDto object</param>
        /// <returns>Response for the request</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAsync([FromBody] VacancyCreateDto vacancyCreateDto)
        {
            try
            {
                var vacancy = _mapper.Map<VacancyCreateDto, Vacancy>(vacancyCreateDto);

                var result = await _vacancyService.AddAsync(vacancy);

                var resource = _mapper.Map<Vacancy, VacancyResponseDto>(result);

                return CreatedAtAction(nameof(GetAsync), new { id = result.Id }, resource);
            }
            catch (ResourceExistException ex)
            {
                return Conflict(ex.Message);
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
        /// Updates Vacancy
        /// </summary>
        /// <param name="id">id of Vacancy</param>
        /// <param name="vacancyUpdateDto">VacancyUpdateDto object</param>
        /// <returns>Response for the request</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] VacancyUpdateDto vacancyUpdateDto)
        {
            try
            {
                var vacancy = _mapper.Map<VacancyUpdateDto, Vacancy>(vacancyUpdateDto);

                var result = await _vacancyService.UpdateAsync(id, vacancy);

                var resource = _mapper.Map<Vacancy, VacancyResponseDto>(result);

                return Ok(resource);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ResourceExistException ex)
            {
                return Conflict(ex.Message);
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
        /// Deletes Vacancy
        /// </summary>
        /// <param name="id">id of Vacancy</param>
        /// <returns>Response for the request</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var result = await _vacancyService.DeleteAsync(id);

                var resource = _mapper.Map<Vacancy, VacancyResponseDto>(result);

                return Ok(resource);
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
