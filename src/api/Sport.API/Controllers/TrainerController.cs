using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sport.API.DTOs.TrainerDto;
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
    public class TrainerController : ControllerBase
    {
        private readonly ITrainerService _trainerService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public TrainerController(ITrainerService trainerService, IMapper mapper, ILogger logger)
        {
            _trainerService = trainerService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Gets all trainers
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync()
        {
            var trainers = await _trainerService.GetAllAsync();

            var response = _mapper.Map<IEnumerable<Trainer>, IEnumerable<TrainerResponseDto>>(trainers);

            return Ok(response);
        }

        /// <summary>
        /// Gets all trainers
        /// </summary>
        /// <param name="complexId">Id of Complex</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetByComplexId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByComplexIdAsync(int complexId)
        {
            var trainers = await _trainerService.GetByComplexIdAsync(complexId);

            var response = _mapper.Map<IEnumerable<Trainer>, IEnumerable<TrainerResponseDto>>(trainers);

            return Ok(response);
        }

        /// <summary>
        /// Gets a trainer
        /// </summary>
        /// <param name="id">Id of Trainer</param>
        /// <returns>Returns TrainerDto object</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync(int id)
        {
            var trainer = await _trainerService.GetAsync(id);

            if (trainer == null)
                return NotFound();

            var response = _mapper.Map<Trainer, TrainerResponseDto>(trainer);

            return Ok(response);
        }

        /// <summary>
        /// Gets trainers by sportTypeId
        /// </summary>
        /// <param name="complexId">Id of Complex</param>
        /// <param name="sportTypeId">Id of SportType</param>
        /// <returns>List of trainers</returns>
        [HttpGet]
        [Route("GetBySportTypeId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBySportTypeIdAsync(int complexId, int sportTypeId)
        {
            var trainers = await _trainerService.GetBySportTypeIdAsync(complexId, sportTypeId);

            var response = _mapper.Map<IEnumerable<Trainer>, IEnumerable<TrainerResponseDto>>(trainers);

            return Ok(response);
        }

        /// <summary>
        /// Gets trainer by employeeId
        /// </summary>
        /// <param name="complexId">Id of Complex</param>
        /// <param name="employeeId">Id of Employee</param>
        /// <returns>TrainerDto object</returns>
        [HttpGet]
        [Route("GetByEmployeeId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByEmployeeIdAsync(int complexId, int employeeId)
        {
            var trainer = await _trainerService.GetByEmployeeIdAsync(complexId, employeeId);

            var response = _mapper.Map<Trainer, TrainerResponseDto>(trainer);

            return Ok(response);
        }

        /// <summary>
        /// Creates new trainer
        /// </summary>
        /// <param name="trainerCreateDto">TrainerCreateDto object</param>
        /// <returns>Response for the request</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAsync([FromBody] TrainerCreateDto trainerCreateDto)
        {
            try
            {
                var trainer = _mapper.Map<TrainerCreateDto, Trainer>(trainerCreateDto);
                var result = await _trainerService.AddAsync(trainer);

                var response = _mapper.Map<Trainer, TrainerResponseDto>(result);

                return CreatedAtAction(nameof(GetAsync), new { id = result.Id }, response);
            }
            catch (ResourceExistException ex)
            {
                return Conflict(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
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
        /// Updates existing trainer
        /// </summary>
        /// <param name="trainerUpdateDto">TrainerUpdateDto object</param>
        /// <returns>Response for the request</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] TrainerUpdateDto trainerUpdateDto)
        {
            try
            {
                var trainer = _mapper.Map<TrainerUpdateDto, Trainer>(trainerUpdateDto);
                var result = await _trainerService.UpdateAsync(id, trainer);

                var response = _mapper.Map<Trainer, TrainerResponseDto>(result);

                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
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
        /// Deletes existing trainer
        /// </summary>
        /// <param name="id">ID of Trainer</param>
        /// <returns>Response for the request</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var result = await _trainerService.DeleteAsync(id);

                var response = _mapper.Map<Trainer, TrainerResponseDto>(result);

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
    }
}
