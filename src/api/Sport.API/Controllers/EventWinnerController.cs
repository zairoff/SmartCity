using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sport.API.DTOs.EventWinnerDto;
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
    public class EventWinnerController : ControllerBase
    {
        private readonly IEventWinnerService _eventWinnerService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public EventWinnerController(IEventWinnerService eventWinnerService, IMapper mapper, ILogger logger)
        {
            _eventWinnerService = eventWinnerService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Gets all Winners
        /// <param name="eventId">id of Event</param>
        /// </summary>
        /// <returns>List of Winners</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync()
        {
            var winners = await _eventWinnerService.GetAllAsync();

            var resource = _mapper.Map<IEnumerable<EventWinner>, IEnumerable<EventWinnerResponseDto>>(winners);

            return Ok(resource);
        }

        /// <summary>
        /// Gets all Winners
        /// <param name="eventId">id of Event</param>
        /// </summary>
        /// <returns>List of Winners</returns>
        [HttpGet]
        [Route("GetByEventId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByEventIdAsync(int eventId)
        {
            var winners = await _eventWinnerService.GetByEventIdAsync(eventId);

            var resource = _mapper.Map<IEnumerable<EventWinner>, IEnumerable<EventWinnerResponseDto>>(winners);

            return Ok(resource);
        }

        /// <summary>
        /// Gets EventWinner
        /// </summary>
        /// <param name="id">id of EventWinner</param>
        /// <returns>EventWinner object</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync(int id)
        {
            var winner = await _eventWinnerService.GetAsync(id);

            if (winner == null)
                return NotFound();

            var resource = _mapper.Map<EventWinner, EventWinnerResponseDto>(winner);

            return Ok(resource);
        }

        /// <summary>
        /// Creates an EventWinner
        /// </summary>
        /// <param name="eventWinnerCreateDto">EventWinnerDto object</param>
        /// <returns>Response for the request</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAsync([FromBody] EventWinnerCreateDto eventWinnerCreateDto)
        {
            try
            {
                var winner = _mapper.Map<EventWinnerCreateDto, EventWinner>(eventWinnerCreateDto);

                var result = await _eventWinnerService.AddAsync(winner);

                var resource = _mapper.Map<EventWinner, EventWinnerResponseDto>(result);

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
        /// Deletes EventWinner
        /// </summary>
        /// <param name="id">id of EventWinner</param>
        /// <returns>Response for the request</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var result = await _eventWinnerService.DeleteAsync(id);

                var resource = _mapper.Map<EventWinner, EventWinnerResponseDto>(result);

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
