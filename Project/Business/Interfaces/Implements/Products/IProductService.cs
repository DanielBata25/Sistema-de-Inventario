using Business.Interfaces.IBusiness;
using Entity.DTOs.Products;


namespace Business.Interfaces.Implements.Products
{
    public interface IProductService : IBusiness<ProductCreateDto, ProductUpdateDto, ProductSelectDto>
    {
        Task<ProductSelectDto?> GetByCodigoAsync(string codigo);
    }
}