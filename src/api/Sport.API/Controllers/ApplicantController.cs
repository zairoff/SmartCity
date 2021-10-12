using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sport.API.DTOs.ApplicantDto;
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
    public class ApplicantController : ControllerBase
    {
        private readonly IApplicantService _applicantService;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public ApplicantController(IApplicantService applicantService, IMapper mapper, ILogger logger)
        {
            _applicantService = applicantService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Gets all applicants
        /// </summary>
        /// <returns>List of Applicants who applied for vacancy</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync()
        {
            var applicants = await _applicantService.GetAllAsync();

            var resource = _mapper.Map<IEnumerable<Applicant>, IEnumerable<ApplicantResponseDto>>(applicants);

            return Ok(resource);
        }

        /// <summary>
        /// Gets applicant
        /// </summary>
        /// <param name="id">id of applicant</param>
        /// <returns>Applicant object</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync(int id)
        {
            var applicant = await _applicantService.GetAsync(id);

            if (applicant == null)
                return NotFound();

            var resource = _mapper.Map<Applicant, ApplicantResponseDto>(applicant);

            return Ok(resource);
        }

        /// <summary>
        /// Creates an applicant
        /// </summary>
        /// <param name="applicantCreateDto">applicantCreateDto object</param>
        /// <returns>Response for the request</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAsync([FromBody] ApplicantCreateDto applicantCreateDto)
        {
            try
            {
                var applicant = _mapper.Map<ApplicantCreateDto, Applicant>(applicantCreateDto);

                var result = await _applicantService.AddAsync(applicant);

                var resource = _mapper.Map<Applicant, ApplicantResponseDto>(result);

                return CreatedAtAction(nameof(GetAsync), new { id = result.Id }, resource);
            }
            catch (ResourceExistException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                // to do: change hard coded string Error to smth like enum
                await _logger.WriteAsync(
                                    ControllerContext.ActionDescriptor.ControllerName + ": " +
                                    ControllerContext.ActionDescriptor.ActionName +
                                    "Exception: " + ex.Message);

                return StatusCode(500);
            }
        }

        /// <summary>
        /// Deletes Applicant
        /// </summary>
        /// <param name="id">id of Applicant</param>
        /// <returns>Response for the request</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var result = await _applicantService.DeleteAsync(id);

                var resource = _mapper.Map<Applicant, ApplicantResponseDto>(result);

                return Ok(resource);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // to do, change hard coded string Exception to smth like enum
                await _logger.WriteAsync(
                                    ControllerContext.ActionDescriptor.ControllerName + ": " +
                                    ControllerContext.ActionDescriptor.ActionName +
                                    "Exception: " + ex.Message);

                return StatusCode(500);
            }
        }
    }
}
