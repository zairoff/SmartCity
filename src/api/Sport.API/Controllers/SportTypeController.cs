using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sport.API.DTOs.SportTypeDto;
using Sport.Domain.Models;
using Sport.Infrastructure.Context;
using Sport.Service.Exceptions;
using Sport.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sport.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SportTypeController : ControllerBase
    {
        private readonly ISportTypeService _sportTypeService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public SportTypeController(ISportTypeService sportTypeService, IMapper mapper, ILogger logger)
        {
            _sportTypeService = sportTypeService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Gets all sportTypes
        /// </summary>
        /// <returns>List of sportTypes</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync()
        {
            var sportTypes = await _sportTypeService.GetAllAsync();

            var resource = _mapper.Map<IEnumerable<SportType>, IEnumerable<SportTypeResponseDto>>(sportTypes);

            return Ok(resource);
        }

        /// <summary>
        /// Gets sportType
        /// </summary>
        /// <param name="id">id of sportType</param>
        /// <returns>sportType object</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync(int id)
        {            
            var sportType = await _sportTypeService.GetAsync(id);

            if (sportType == null)
                return NotFound();

            var resource = _mapper.Map<SportType, SportTypeResponseDto>(sportType);

            return Ok(resource);
        }

        /// <summary>
        /// Adds sportType
        /// </summary>
        /// <param name="sportTypeCreateDto">SportTypeCreateDto object</param>
        /// <returns>Response for the request</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAsync([FromBody] SportTypeCreateDto sportTypeCreateDto)
        {
            try
            {
                var sportType = _mapper.Map<SportTypeCreateDto, SportType>(sportTypeCreateDto);

                var result = await _sportTypeService.AddAsync(sportType);

                var resource = _mapper.Map<SportType, SportTypeResponseDto>(result);

                return CreatedAtAction(nameof(GetAsync), new { id = result.Id }, resource);
            }
            catch(ResourceExistException ex)
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
        /// Updates sportType
        /// </summary>
        /// <param name="id">id of sportType</param>
        /// <param name="sportTypeUpdateDto">SportTypeUpdateDto object</param>
        /// <returns>Response for the request</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] SportTypeUpdateDto sportTypeUpdateDto)
        {
            try
            {
                var sportType = _mapper.Map<SportTypeUpdateDto, SportType>(sportTypeUpdateDto);

                var result = await _sportTypeService.UpdateAsync(id, sportType);

                var resource = _mapper.Map<SportType, SportTypeResponseDto>(result);

                return Ok(resource);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch(ResourceExistException ex)
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
        /// Deletes sportType
        /// </summary>
        /// <param name="id">id of sportType</param>
        /// <returns>Response for the request</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var result = await _sportTypeService.DeleteAsync(id);

                var resource = _mapper.Map<SportType, SportTypeResponseDto>(result);

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
