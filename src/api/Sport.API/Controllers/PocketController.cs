using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sport.API.DTOs.PocketDto;
using Sport.Domain.Models;
using Sport.Service.Exceptions;
using Sport.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sport.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PocketController : ControllerBase
    {
        private readonly IPocketService _pocketService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public PocketController(IPocketService pocketService, IMapper mapper, ILogger logger)
        {
            _pocketService = pocketService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Gets all pockets
        /// </summary>
        /// <returns>List of pockets</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAsync()
        {
            var pockets = await _pocketService.GetAllAsync();

            var resource = _mapper.Map<IEnumerable<Pocket>, IEnumerable<PocketResponseDto>>(pockets);

            return Ok(resource);
        }

        /// <summary>
        /// Gets pocket
        /// </summary>
        /// <param name="id">id of pocket</param>
        /// <returns>Pocket object</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync(int id)
        {
            var pocket = await _pocketService.GetAsync(id);

            if (pocket == null)
                return NotFound();

            var resource = _mapper.Map<Pocket, PocketResponseDto>(pocket);

            return Ok(resource);
        }

        /// <summary>
        /// Creates pocket
        /// </summary>
        /// <param name="pocketCreateDto">pocketCreateDto object</param>
        /// <returns>Response for the request</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAsync([FromBody] PocketCreateDto pocketCreateDto)
        {
            try
            {
                var pocket = _mapper.Map<PocketCreateDto, Pocket>(pocketCreateDto);

                var result = await _pocketService.AddAsync(pocket);

                var resource = _mapper.Map<Pocket, PocketResponseDto>(result);

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
        /// Updates Pocket
        /// </summary>
        /// <param name="id">id of Pocket</param>
        /// <param name="pocketEditDto">PocketEditDto object</param>
        /// <returns>Response for the request</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] PocketUpdateDto pocketEditDto)
        {
            try
            {
                var pocket = _mapper.Map<PocketUpdateDto, Pocket>(pocketEditDto);

                var result = await _pocketService.UpdateAsync(id, pocket);

                var resource = _mapper.Map<Pocket, PocketResponseDto>(result);

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
        /// Deletes Pocket
        /// </summary>
        /// <param name="id">id of Pocket</param>
        /// <returns>Response for the request</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var result = await _pocketService.DeleteAsync(id);

                var resource = _mapper.Map<Pocket, PocketResponseDto>(result);

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
