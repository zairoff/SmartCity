using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sport.API.DTOs.EventSubscriberDto;
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
    public class EventSubscriberController : ControllerBase
    {
        private readonly IEventSubscriberService _eventSubscriberService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public EventSubscriberController(IEventSubscriberService eventSubscriberService, IMapper mapper, ILogger logger)
        {
            _eventSubscriberService = eventSubscriberService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Gets all EventSubscribers
        /// </summary>
        /// <returns>List of EventSubscribers</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAsync()
        {
            var subscribers = await _eventSubscriberService.GetAllAsync();

            var resource = _mapper.Map<IEnumerable<EventSubscriber>, IEnumerable<EventSubscriberResponseDto>>(subscribers);

            return Ok(resource);
        }

        /// <summary>
        /// Gets EventSubscriber
        /// </summary>
        /// <param name="id">id of EventSubscriber</param>
        /// <returns>EventSubscriber object</returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync(int id)
        {
            var subscriber = await _eventSubscriberService.GetAsync(id);

            if (subscriber == null)
                return NotFound();

            var resource = _mapper.Map<EventSubscriber, EventSubscriberResponseDto>(subscriber);

            return Ok(resource);
        }

        /// <summary>
        /// Creates an EventSubscriber
        /// </summary>
        /// <param name="eventSubscriberCreateDto">EventSubscriberDto object</param>
        /// <returns>Response for the request</returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("Subscribe")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SubscribeAsync([FromBody] EventSubscriberCreateDto eventSubscriberCreateDto)
        {
            try
            {
                var subscriber = _mapper.Map<EventSubscriberCreateDto, EventSubscriber>(eventSubscriberCreateDto);

                var result = await _eventSubscriberService.AddAsync(subscriber);

                var resource = _mapper.Map<EventSubscriber, EventSubscriberResponseDto>(result);

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
        /// Deletes EventSubscriber
        /// </summary>
        /// <param name="id">id of EventSubscriber</param>
        /// <returns>Response for the request</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var result = await _eventSubscriberService.DeleteAsync(id);

                var resource = _mapper.Map<EventSubscriber, EventSubscriberResponseDto>(result);

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
