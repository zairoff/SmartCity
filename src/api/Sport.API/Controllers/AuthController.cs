using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Sport.API.Auth;
using Sport.API.DTOs.Auth;
using Sport.Service.Exceptions;
using Sport.Service.Interfaces;
using System;
using System.Threading.Tasks;

namespace Sport.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticateService _authenticateService;
        private readonly ILogger _logger;

        public AuthController(IAuthenticateService authenticateService, ILogger logger)
        {
            _authenticateService = authenticateService;
            _logger = logger;
        }

        [HttpPost("Authenticate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Authenticate(UserCredential userCredential)
        {
            try
            {
                var token = _authenticateService.Authenticate(userCredential.Username, userCredential.Password);
                return Ok(token);
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                await _logger.WriteAsync("AuthController: " + ex.Message);
                return StatusCode(500);
            }
        }
    }
}
