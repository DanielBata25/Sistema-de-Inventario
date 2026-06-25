using Business.Interfaces.Implements.Products;
using CRUD_Básico.Controllers.Base;
using Entity.DTOs.Products;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exceptions;

namespace CRUD_Básico.Controllers.Implements
{
    public class ProductController
        : BaseController<ProductCreateDto, ProductUpdateDto, ProductSelectDto, IProductService>
    {
        private readonly IProductService _productService;

        public ProductController(
            IProductService productService,
            ILogger<ProductController> logger) : base(productService, logger)
        {
            _productService = productService;
        }

        [HttpGet("codigo/{codigo}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetByCodigo(string codigo)
        {
            try
            {
                var result = await _productService.GetByCodigoAsync(codigo);

                if (result == null)
                {
                    return NotFound(new { message = $"No se encontró el producto con código {codigo}." });
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
                _logger.LogError(ex, "Error al obtener el producto con código {Codigo}.", codigo);
                return StatusCode(500, new { message = "Error interno del servidor." });
            }
        }
    }
}