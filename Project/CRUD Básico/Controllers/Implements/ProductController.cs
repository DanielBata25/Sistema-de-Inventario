using Business.Interfaces.Implements.Products;
using CRUD_Básico.Controllers.Base;
using Entity.DTOs.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_Básico.Controllers.Implements
{
    [Authorize]
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

        [Authorize]
        [HttpGet("codigo/{codigo}")]
        [ProducesResponseType(200)]
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el producto con código {Codigo}.", codigo);
                return StatusCode(500, new { message = "Error interno del servidor." });
            }
        }

        [Authorize(Roles = "Admin,Employee")]
        [HttpPost]
        public override async Task<IActionResult> Post([FromBody] ProductCreateDto dto)
        {
            return await base.Post(dto);
        }

        [Authorize(Roles = "Admin,Employee")]
        [HttpPut("{id:int}")]
        public override async Task<IActionResult> Put(int id, [FromBody] ProductUpdateDto dto)
        {
            return await base.Put(id, dto);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public override async Task<IActionResult> Delete(int id)
        {
            return await base.Delete(id);
        }
    }
}