using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sport.API.DTOs.SportGroupDto;
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
    public class SportGroupController : ControllerBase
    {
        private readonly ISportGroupService _sportGroupService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public SportGroupController(ISportGroupService groupService, IMapper mapper, ILogger logger)
        {
            _sportGroupService = groupService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Gets all groups
        /// </summary>
        /// <returns>List of groups</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync()
        {
            var groups = await _sportGroupService.GetAllAsync();

            var resource = _mapper.Map<IEnumerable<SportGroup>, IEnumerable<SportGroupResponseDto>>(groups);

            return Ok(resource);
        }

        /// <summary>
        /// Get a group
        /// </summary>
        /// <returns>Group object</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync(int id)
        {
            var group = await _sportGroupService.GetAsync(id);

            var resource = _mapper.Map<SportGroup, SportGroupResponseDto>(group);

            return Ok(resource);
        }

        /// <summary>
        /// Creates a group
        /// </summary>
        /// <param name="sportGroupCreateDto">sportGroupCreateDto object</param>
        /// <returns>Response for the request</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAsync([FromBody] SportGroupCreateDto sportGroupCreateDto)
        {
            try
            {
                var group = _mapper.Map<SportGroupCreateDto, SportGroup>(sportGroupCreateDto);

                var result = await _sportGroupService.AddAsync(group);

                var resource = _mapper.Map<SportGroup, SportGroupResponseDto>(result);

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
        /// Updates Group
        /// </summary>
        /// <param name="id">id of Group</param>
        /// <param name="sportGroupUpdateDto">SportGroupUpdateDto object</param>
        /// <returns>Response for the request</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] SportGroupUpdateDto sportGroupUpdateDto)
        {
            try
            {
                var group = _mapper.Map<SportGroupUpdateDto, SportGroup>(sportGroupUpdateDto);

                var result = await _sportGroupService.UpdateAsync(id, group);

                var resource = _mapper.Map<SportGroup, SportGroupResponseDto>(result);

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
        /// Deletes Group
        /// </summary>
        /// <param name="id">id of Group</param>
        /// <returns>Response for the request</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var result = await _sportGroupService.DeleteAsync(id);

                var resource = _mapper.Map<SportGroup, SportGroupResponseDto>(result);

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
