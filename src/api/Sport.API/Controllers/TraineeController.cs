using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sport.API.DTOs.TraineeDto;
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
    public class TraineeController : ControllerBase
    {
        private readonly ITraineeService _traineeService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public TraineeController(ITraineeService traineeService, IMapper mapper, ILogger logger)
        {
            _traineeService = traineeService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Gets all trainees
        /// </summary>
        /// <param name="complexId">ID of Complex</param>
        /// <returns>List of trainees</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync()
        {
            var trainees = await _traineeService.GetAllAsync();

            var response = _mapper.Map<IEnumerable<Trainee>, IEnumerable<TraineeResponseDto>>(trainees);

            return Ok(response);
        }

        /// <summary>
        /// Gets all trainees
        /// </summary>
        /// <param name="complexId">ID of Complex</param>
        /// <returns>List of trainees</returns>
        [HttpGet]
        [Route("GetByComplexId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByComplexIdAsync(int complexId)
        {
            var trainees = await _traineeService.GetByComplexIdAsync(complexId);

            var response = _mapper.Map<IEnumerable<Trainee>, IEnumerable<TraineeResponseDto>>(trainees);

            return Ok(response);
        }

        /// <summary>
        /// Gets a trainee
        /// </summary>
        /// <param name="id">ID of trainee</param>
        /// <returns>A trainee</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync(int id)
        {
            var trainee = await _traineeService.GetAsync(id);

            if (trainee == null)
                return NotFound();

            var response = _mapper.Map<Trainee, TraineeResponseDto>(trainee);

            return Ok(response);
        }

        /// <summary>
        /// Gets list of Trainees
        /// </summary>
        /// <param name="complexId">Id of Complex</param>
        /// <param name="groupdId">Id of group</param>
        /// <returns>List of Trainees</returns>
        [HttpGet]
        [Route("GetByGroupId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByGroupIdAsync(int complexId, int groupdId)
        {
            var trainees = await _traineeService.GetByGroupIdAsync(complexId, groupdId);

            var response = _mapper.Map<IEnumerable<Trainee>, IEnumerable<TraineeResponseDto>>(trainees);

            return Ok(response);
        }

        /// <summary>
        /// Gets list of Trainees
        /// </summary>
        /// <param name="complexId">ID of Complex</param>
        /// <param name="isPaid">boolean type for payment</param>
        /// <returns>List of Trainees</returns>
        [HttpGet]
        [Route("GetByPaymentStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByPaymentStatusAsync(int complexId, bool isPaid)
        {
            var trainees = await _traineeService.GetByPaymentStatusAsync(complexId, isPaid);

            var response = _mapper.Map<IEnumerable<Trainee>, IEnumerable<TraineeResponseDto>>(trainees);

            return Ok(response);
        }

        /// <summary>
        /// Gets list of Trainees
        /// </summary>
        /// <param name="complexId">ID of Complex</param>
        /// <param name="pocketId">ID of pocket</param>
        /// <returns>List of Trainees</returns>
        [HttpGet]
        [Route("GetByPocketId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByPocketIdAsync(int complexId, int pocketId)
        {
            var trainees = await _traineeService.GetByPocketIdAsync(complexId, pocketId);

            var response = _mapper.Map<IEnumerable<Trainee>, IEnumerable<TraineeResponseDto>>(trainees);

            return Ok(response);
        }

        /// <summary>
        /// Gets a trainee
        /// </summary>
        /// <param name="complexId">ID of Complex</param>
        /// <param name="personId">ID of person</param>
        /// <returns>A trainee</returns>
        [HttpGet]
        [Route("GetByPersonId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByPersonIdAsync(int complexId, string personId)
        {
            var trainee = await _traineeService.GetByPersonIdAsync(complexId, personId);

            if (trainee == null)
                return NotFound();

            var response = _mapper.Map<Trainee, TraineeResponseDto>(trainee);

            return Ok(response);
        }

        /// <summary>
        /// Creates new trainee
        /// </summary>
        /// <param name="traineeCreateDto">TraineeCreateDto object</param>
        /// <returns>Response for the request</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAsync([FromBody] TraineeCreateDto traineeCreateDto)
        {
            try
            {
                var trainer = _mapper.Map<TraineeCreateDto, Trainee>(traineeCreateDto);
                var result = await _traineeService.AddAsync(trainer);

                var response = _mapper.Map<Trainee, TraineeResponseDto>(result);

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
        /// <param name="traineeUpdateDto">TraineeUpdateDto object</param>
        /// <returns>Response for the request</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] TraineeUpdateDto traineeUpdateDto)
        {
            try
            {
                var trainee = _mapper.Map<TraineeUpdateDto, Trainee>(traineeUpdateDto);
                var result = await _traineeService.UpdateAsync(id, trainee);

                var response = _mapper.Map<Trainee, TraineeResponseDto>(result);

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
        /// Deletes existing trainee
        /// </summary>
        /// <param name="id">ID of Trainee</param>
        /// <returns>Response for the request</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var result = await _traineeService.DeleteAsync(id);

                var response = _mapper.Map<Trainee, TraineeResponseDto>(result);

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
