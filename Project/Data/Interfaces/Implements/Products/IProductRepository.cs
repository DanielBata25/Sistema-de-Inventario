using Data.Interfaces.IDataGeneric;
using Entity.Model;


namespace Data.Interfaces.IProductRepository
{
    public interface IProductRepository : IDataGeneric<Product>
    {
        Task<Product?> GetByCodigoAsync(string codigo);
    }
}