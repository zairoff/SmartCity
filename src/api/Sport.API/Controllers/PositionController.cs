using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sport.API.DTOs.PositionDto;
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
    public class PositionController : ControllerBase
    {
        private readonly IPositionService _positionService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public PositionController(IPositionService positionService, IMapper mapper, ILogger logger)
        {
            _positionService = positionService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Gets all positions
        /// </summary>
        /// <returns>List of positions</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]        
        public async Task<IActionResult> GetAsync()
        {
            var positions = await _positionService.GetAllAsync();

            var resource = _mapper.Map<IEnumerable<Position>, IEnumerable<PositionResponseDto>>(positions);

            return Ok(resource);
        }

        /// <summary>
        /// Gets an position
        /// </summary>
        /// <param name="id">id of position</param>
        /// <returns>An Position object</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync(int id)
        {
            var position = await _positionService.GetAsync(id);

            if (position == null)
                return NotFound();

            var resource = _mapper.Map<Position, PositionResponseDto>(position);

            return Ok(resource);
        }

        /// <summary>
        /// Adds a Position
        /// </summary>
        /// <param name="positionCreateDto">PositionCreateDto object</param>
        /// <returns>Response for the request</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAsync([FromBody] PositionCreateDto positionCreateDto)
        {
            try
            {
                var position = _mapper.Map<PositionCreateDto, Position>(positionCreateDto);

                var result = await _positionService.AddAsync(position);

                var resource = _mapper.Map<Position, PositionResponseDto>(result);

                return CreatedAtAction(nameof(GetAsync), new { id = result.Id}, resource);
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
        /// Updates Position
        /// </summary>
        /// <param name="id">id of Position</param>
        /// <param name="positionUpdateDto">PositionUpdateDto object</param>
        /// <returns>Response for the request</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] PositionUpdateDto positionUpdateDto)
        {
            try
            {
                var position = _mapper.Map<PositionUpdateDto, Position>(positionUpdateDto);

                var result = await _positionService.UpdateAsync(id, position);

                var resource = _mapper.Map<Position, PositionResponseDto>(result);

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
        /// Deletes Position
        /// </summary>
        /// <param name="id">id of Position</param>
        /// <returns>Response for the request</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var result = await _positionService.DeleteAsync(id);

                var resource = _mapper.Map<Position, PositionResponseDto>(result);

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
