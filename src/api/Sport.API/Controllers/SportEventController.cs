using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sport.API.DTOs.SportEventDto;
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
    public class SportEventController : ControllerBase
    {
        private readonly ISportEventService _sportEventService;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public SportEventController(ISportEventService sportEventService, IMapper mapper, ILogger logger, INotificationService notificationService)
        {
            _sportEventService = sportEventService;
            _notificationService = notificationService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Gets all SportEvents
        /// </summary>
        /// <returns>List of SportEvents</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync()
        {
            var events = await _sportEventService.GetAllAsync();

            var resource = _mapper.Map<IEnumerable<SportEvent>, IEnumerable<SportEventResponseDto>>(events);

            return Ok(resource);
        }

        /// <summary>
        /// Gets all SportEvents
        /// </summary>
        /// <returns>List of SportEvents</returns>
        [HttpGet]
        [Route("GetByComplexId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByComplexIdAsync(int complexId)
        {
            var events = await _sportEventService.GetByComplexIdAsync(complexId);

            var resource = _mapper.Map<IEnumerable<SportEvent>, IEnumerable<SportEventResponseDto>>(events);

            return Ok(resource);
        }

        /// <summary>
        /// Gets SportEvent
        /// </summary>
        /// <param name="id">id of SportEvent</param>
        /// <returns>SportEvent object</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync(int id)
        {
            var sportEvent = await _sportEventService.GetAsync(id);

            if (sportEvent == null)
                return NotFound();

            var resource = _mapper.Map<SportEvent, SportEventResponseDto>(sportEvent);

            return Ok(resource);
        }

        /// <summary>
        /// Creates an SportEvent
        /// </summary>
        /// <param name="sportEventCreateDto">sportEventCreateDto object</param>
        /// <returns>Response for the request</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAsync([FromBody] SportEventCreateDto sportEventCreateDto)
        {
            try
            {
                var sportEvent = _mapper.Map<SportEventCreateDto, SportEvent>(sportEventCreateDto);

                var result = await _sportEventService.AddAsync(sportEvent);

                var resource = _mapper.Map<SportEvent, SportEventResponseDto>(result);

                // Notify other modules about sport event
                await _notificationService.SendAsync(sportEvent);

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
        /// Updates existing SportEvent
        /// </summary>
        /// <param name="id">id of SportEvent</param>
        /// <param name="sportEventUpdateDto">sportEventUpdateDto object</param>
        /// <returns>Response fot the request</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] SportEventUpdateDto sportEventUpdateDto)
        {
            try
            {
                var employee = _mapper.Map<SportEventUpdateDto, SportEvent>(sportEventUpdateDto);
                var result = await _sportEventService.UpdateAsync(id, employee);

                var response = _mapper.Map<SportEvent, SportEventResponseDto>(result);

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
        /// Deletes SportEvent
        /// </summary>
        /// <param name="id">id of SportEvent</param>
        /// <returns>Response for the request</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var result = await _sportEventService.DeleteAsync(id);

                var resource = _mapper.Map<SportEvent, SportEventResponseDto>(result);

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
