using Business.Interfaces.Implements.Products;
using Business.Repository;
using Data.Interfaces.Implements.Products;
using Data.Interfaces.IRepository;
using Entity.DTOs.Products;
using Entity.Model;
using MapsterMapper;
using Utilities.Exceptions;
using Utilities.Helpers.Business;

namespace Business.Services.Products
{
    public class ProductService
        : BusinessGeneric<ProductCreateDto, ProductUpdateDto, ProductSelectDto, Product>, IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(
            IDataGeneric<Product> data,
            IMapper mapper,
            IProductRepository productRepository) : base(data, mapper)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductSelectDto?> GetByCodigoAsync(string codigo)
        {
            BusinessValidationHelper.ThrowIfNullOrEmpty(codigo, "El código del producto es obligatorio.");

            var product = await _productRepository.GetByCodigoAsync(codigo.Trim());

            if (product == null)
            {
                return null;
            }

            return Mapper.Map<ProductSelectDto>(product);
        }

        public override async Task<ProductSelectDto> CreateAsync(ProductCreateDto dto)
        {
            BusinessValidationHelper.ThrowIfNull(dto, "El producto no puede ser nulo.");

            ValidateProduct(dto.Codigo, dto.Nombre, dto.Precio, dto.Stock);

            var existingProduct = await _productRepository.GetByCodigoAsync(dto.Codigo.Trim());

            if (existingProduct != null)
            {
                throw new BusinessRuleViolationException("Ya existe un producto con este código.");
            }

            var product = Mapper.Map<Product>(dto);

            product.Codigo = dto.Codigo.Trim();
            product.Nombre = dto.Nombre.Trim();
            product.Activo = true;
            product.FechaCreacion = DateTime.Now;

            var created = await _productRepository.AddAsync(product);

            return Mapper.Map<ProductSelectDto>(created);
        }

        public override async Task<ProductSelectDto?> UpdateAsync(int id, ProductUpdateDto dto)
        {
            BusinessValidationHelper.ThrowIfZeroOrLess(id, "El ID debe ser mayor que cero.");
            BusinessValidationHelper.ThrowIfNull(dto, "El producto no puede ser nulo.");

            ValidateProduct(dto.Codigo, dto.Nombre, dto.Precio, dto.Stock);

            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
            {
                return null;
            }

            var existingProduct = await _productRepository.GetByCodigoAsync(dto.Codigo.Trim());

            if (existingProduct != null && existingProduct.Id != id)
            {
                throw new BusinessRuleViolationException("Ya existe otro producto con este código.");
            }

            product.Codigo = dto.Codigo.Trim();
            product.Nombre = dto.Nombre.Trim();
            product.Descripcion = dto.Descripcion;
            product.Precio = dto.Precio;
            product.Stock = dto.Stock;
            product.Activo = dto.Activo;
            product.FechaActualizacion = DateTime.Now;

            var updated = await _productRepository.UpdateAsync(product);

            return Mapper.Map<ProductSelectDto>(updated);
        }

        private static void ValidateProduct(string codigo, string nombre, decimal precio, int stock)
        {
            BusinessValidationHelper.ThrowIfNullOrEmpty(codigo, "El código del producto es obligatorio.");
            BusinessValidationHelper.ThrowIfNullOrEmpty(nombre, "El nombre del producto es obligatorio.");
            BusinessValidationHelper.ThrowIfNegative(precio, "El precio no puede ser negativo.");
            BusinessValidationHelper.ThrowIfNegative(stock, "El stock no puede ser negativo.");
        }
    }
}