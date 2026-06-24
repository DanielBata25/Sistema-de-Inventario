using Data.Interfaces.IRepository;
using Entity.Model;

namespace Data.Interfaces.Implements.Products
{
    public interface IProductRepository : IDataGeneric<Product>
    {
        Task<Product?> GetByCodigoAsync(string codigo);
    }
}