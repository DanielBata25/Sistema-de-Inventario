using Business.Interfaces.Implements.Products;
using Business.Repository;
using Data.Interfaces.Implements.Products;
using Data.Interfaces.IRepository;
using Entity.DTOs.Products;
using Entity.Model;
using MapsterMapper;

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
            if (string.IsNullOrWhiteSpace(codigo))
            {
                throw new ArgumentException("El código del producto es obligatorio.");
            }

            var product = await _productRepository.GetByCodigoAsync(codigo.Trim());

            if (product == null)
            {
                return null;
            }

            return Mapper.Map<ProductSelectDto>(product);
        }

        public override async Task<ProductSelectDto> CreateAsync(ProductCreateDto dto)
        {
            ValidateProduct(dto.Codigo, dto.Nombre, dto.Precio, dto.Stock);

            var existingProduct = await _productRepository.GetByCodigoAsync(dto.Codigo.Trim());

            if (existingProduct != null)
            {
                throw new InvalidOperationException("Ya existe un producto con este código.");
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
            if (id <= 0)
            {
                throw new ArgumentException("El ID debe ser mayor que cero.");
            }

            ValidateProduct(dto.Codigo, dto.Nombre, dto.Precio, dto.Stock);

            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
            {
                return null;
            }

            var existingProduct = await _productRepository.GetByCodigoAsync(dto.Codigo.Trim());

            if (existingProduct != null && existingProduct.Id != id)
            {
                throw new InvalidOperationException("Ya existe otro producto con este código.");
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
            if (string.IsNullOrWhiteSpace(codigo))
            {
                throw new ArgumentException("El código del producto es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("El nombre del producto es obligatorio.");
            }

            if (precio < 0)
            {
                throw new ArgumentException("El precio no puede ser negativo.");
            }

            if (stock < 0)
            {
                throw new ArgumentException("El stock no puede ser negativo.");
            }
        }
    }
}