using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sport.API.DTOs.EventParticipantDto;
using Sport.Domain.Models;
using Sport.Service.Exceptions;
using Sport.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EventParticipantController : ControllerBase
    {
        private readonly IEventParticipantService _eventParticipantService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public EventParticipantController(IEventParticipantService eventParticipantService, IMapper mapper, ILogger logger)
        {
            _eventParticipantService = eventParticipantService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Gets all participants
        /// </summary>
        /// <returns>List of Participants</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync()
        {
            var eventParticipants = await _eventParticipantService.GetAllAsync();

            var resource = _mapper.Map<IEnumerable<EventParticipant>, IEnumerable<EventParticipantResponseDto>>(eventParticipants);

            return Ok(resource);
        }

        /// <summary>
        /// Gets EventParticipant
        /// </summary>
        /// <param name="id">id of EventParticipant</param>
        /// <returns>EventParticipant object</returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync(int id)
        {
            var eventParticipant = await _eventParticipantService.GetAsync(id);

            if (eventParticipant == null)
                return NotFound();

            var resource = _mapper.Map<EventParticipant, EventParticipantResponseDto>(eventParticipant);

            return Ok(resource);
        }

        /// <summary>
        /// Gets all participants
        /// </summary>
        /// <param name="eventId">id of event</param>
        /// <returns>List of Participants</returns>
        [HttpGet]
        [Route("GetByEventId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByEventIdAsync(int eventId)
        {
            var eventParticipants = await _eventParticipantService.GetByEventIdAsync(eventId);

            var resource = _mapper.Map<IEnumerable<EventParticipant>, IEnumerable<EventParticipantResponseDto>>(eventParticipants);

            return Ok(resource);
        }

        /// <summary>
        /// Creates an EventParticipant
        /// </summary>
        /// <param name="eventParticipantCreateDto">EventParticipantCreateDto object</param>
        /// <returns>Response for the request</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAsync([FromBody] EventParticipantCreateDto eventParticipantCreateDto)
        {
            try
            {
                var eventParticipant = _mapper.Map<EventParticipantCreateDto, EventParticipant>(eventParticipantCreateDto);

                var result = await _eventParticipantService.AddAsync(eventParticipant);

                var resource = _mapper.Map<EventParticipant, EventParticipantResponseDto>(result);

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
        /// Deletes EventParticipant
        /// </summary>
        /// <param name="id">id of EventParticipant</param>
        /// <returns>Response for the request</returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var result = await _eventParticipantService.DeleteAsync(id);

                var resource = _mapper.Map<EventParticipant, EventParticipantResponseDto>(result);

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
