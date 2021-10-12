using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sport.API.DTOs.TrainerGroupDto;
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
    public class TrainerGroupController : ControllerBase
    {
        private readonly ITrainerGroupService _trainerGroupService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public TrainerGroupController(ITrainerGroupService trainerGroupService, IMapper mapper, ILogger logger)
        {
            _trainerGroupService = trainerGroupService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Gets all trainerGroups
        /// </summary>
        /// <returns>List of trainerGroups</returns>
        [HttpGet]
        [Route("GetAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync()
        {
            var trainerGroups = await _trainerGroupService.GetAllAsync();

            var resource = _mapper.Map<IEnumerable<TrainerGroup>, IEnumerable<TrainerGroupResponseDto>>(trainerGroups);

            return Ok(resource);
        }

        /// <summary>
        /// Gets all trainerGroups by groupId
        /// <param name="groupId">id of SportGroup</param>
        /// </summary>
        /// <returns>List of trainerGroups</returns>
        [HttpGet]
        [Route("GetByGroupIdAsync/{groupId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByGroupIdAsync(int groupId)
        {
            var trainerGroups = await _trainerGroupService.GetByGroupIdAsync(groupId);

            var resource = _mapper.Map<IEnumerable<TrainerGroup>, IEnumerable<TrainerGroupResponseDto>>(trainerGroups);

            return Ok(resource);
        }

        /// <summary>
        /// Gets all trainerGroups by trainerId
        /// <param name="trainerId">id of Trainer</param>
        /// </summary>
        /// <returns>List of trainerGroups</returns>
        [HttpGet]
        [Route("GetByTrainerIdAsync/{trainerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByTrainerIdAsync(int trainerId)
        {
            var trainerGroups = await _trainerGroupService.GetByTrainerIdAsync(trainerId);

            var resource = _mapper.Map<IEnumerable<TrainerGroup>, IEnumerable<TrainerGroupResponseDto>>(trainerGroups);

            return Ok(resource);
        }

        /// <summary>
        /// Gets trainerGroup
        /// </summary>
        /// <param name="id">id of trainerGroup</param>
        /// <returns>trainerGroup object</returns>
        [HttpGet]
        [Route("GetAsync/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync(int id)
        {
            var trainerGroup = await _trainerGroupService.GetAsync(id);

            if (trainerGroup == null)
                return NotFound();

            var resource = _mapper.Map<TrainerGroup, TrainerGroupResponseDto>(trainerGroup);

            return Ok(resource);
        }

        /// <summary>
        /// Enrolles a trainer to spesific group
        /// </summary>
        /// <param name="trainerGroupCreateDto">trainerGroupCreateDto object</param>
        /// <returns>Response for the request</returns>
        [HttpPost]
        [Route("CreateAsync")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAsync([FromBody] TrainerGroupCreateDto trainerGroupCreateDto)
        {
            try
            {
                var trainerGroup = _mapper.Map<TrainerGroupCreateDto, TrainerGroup>(trainerGroupCreateDto);

                var result = await _trainerGroupService.AddAsync(trainerGroup);

                var resource = _mapper.Map<TrainerGroup, TrainerGroupResponseDto>(result);

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
        /// Deletes TrainerGroup
        /// </summary>
        /// <param name="id">id of TrainerGroup</param>
        /// <returns>Response for the request</returns>
        [HttpDelete]
        [Route("DeleteAsync/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var result = await _trainerGroupService.DeleteAsync(id);

                var resource = _mapper.Map<TrainerGroup, TrainerGroupResponseDto>(result);

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
