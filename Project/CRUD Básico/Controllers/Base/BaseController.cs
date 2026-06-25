using Business.Interfaces.IBusiness;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exceptions;

namespace CRUD_Básico.Controllers.Base
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public abstract class BaseController<TCreateDto, TUpdateDto, TSelectDto, TService> : ControllerBase
        where TCreateDto : class
        where TUpdateDto : class
        where TSelectDto : class
        where TService : IBusiness<TCreateDto, TUpdateDto, TSelectDto>
    {
        protected readonly TService _service;
        protected readonly ILogger _logger;

        protected BaseController(TService service, ILogger logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public virtual async Task<IActionResult> Get()
        {
            try
            {
                var result = await _service.GetAllAsync();
                return Ok(result);
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning(ex, "Error de negocio obteniendo datos.");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo datos.");
                return StatusCode(500, new { message = "Error interno del servidor." });
            }
        }

        [HttpGet("{id:int}")]
        public virtual async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);

                if (result == null)
                {
                    return NotFound(new { message = $"No se encontró el elemento con ID {id}." });
                }

                return Ok(result);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el registro con ID {Id}.", id);
                return StatusCode(500, new { message = "Error interno del servidor." });
            }
        }

        [HttpPost]
        public virtual async Task<IActionResult> Post([FromBody] TCreateDto dto)
        {
            try
            {
                var result = await _service.CreateAsync(dto);
                return Ok(result);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (BusinessRuleViolationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el registro.");
                return StatusCode(500, new { message = "Error interno del servidor." });
            }
        }

        [HttpPut("{id:int}")]
        public virtual async Task<IActionResult> Put(int id, [FromBody] TUpdateDto dto)
        {
            try
            {
                var result = await _service.UpdateAsync(id, dto);

                if (result == null)
                {
                    return NotFound(new { message = $"No se encontró el elemento con ID {id}." });
                }

                return Ok(result);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (BusinessRuleViolationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el registro con ID {Id}.", id);
                return StatusCode(500, new { message = "Error interno del servidor." });
            }
        }

        [HttpDelete("{id:int}")]
        public virtual async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _service.DeleteAsync(id);

                if (!deleted)
                {
                    return NotFound(new { message = $"No se encontró el elemento con ID {id}." });
                }

                return NoContent();
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el registro con ID {Id}.", id);
                return StatusCode(500, new { message = "Error interno del servidor." });
            }
        }
    }
}