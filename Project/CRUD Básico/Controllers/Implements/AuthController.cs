using Business.Interfaces.Implements.Auth;
using Entity.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_Básico.Controllers.Implements
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IAuthService authService,
            ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                var result = await _authService.LoginAsync(dto);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al iniciar sesión.");
                return StatusCode(500, new { message = "Error interno del servidor." });
            }
        }

        [HttpPost("refresh-token")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto dto)
        {
            try
            {
                var result = await _authService.RefreshTokenAsync(dto);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al renovar el token.");
                return StatusCode(500, new { message = "Error interno del servidor." });
            }
        }

        [HttpPost("logout")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Logout([FromBody] LogoutDto dto)
        {
            try
            {
                var result = await _authService.LogoutAsync(dto);

                if (!result)
                {
                    return BadRequest(new { message = "No se pudo cerrar la sesión. Refresh token inválido." });
                }

                return Ok(new { message = "Sesión cerrada correctamente." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cerrar sesión.");
                return StatusCode(500, new { message = "Error interno del servidor." });
            }
        }
    }
}