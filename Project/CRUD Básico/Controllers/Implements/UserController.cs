using Business.Interfaces.Implements.Users;
using CRUD_Básico.Controllers.Base;
using Entity.DTOs.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_Básico.Controllers.Implements
{
    [Authorize(Roles = "Admin")]
    public class UserController
        : BaseController<UserCreateDto, UserUpdateDto, UserSelectDto, IUserService>
    {
        private readonly IUserService _userService;

        public UserController(
            IUserService userService,
            ILogger<UserController> logger) : base(userService, logger)
        {
            _userService = userService;
        }

        [HttpGet("email/{email}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetByEmail(string email)
        {
            try
            {
                var result = await _userService.GetByEmailAsync(email);

                if (result == null)
                {
                    return NotFound(new { message = $"No se encontró el usuario con correo {email}." });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el usuario con correo {Email}.", email);
                return StatusCode(500, new { message = "Error interno del servidor." });
            }
        }
    }
}