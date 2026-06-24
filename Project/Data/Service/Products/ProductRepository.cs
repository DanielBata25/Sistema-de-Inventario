using Data.Interfaces.Implements.Products;
using Data.Repository;
using Entity.Infrastructure.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;

namespace Data.Service.Products
{
    public class ProductRepository : DataGeneric<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Product?> GetByCodigoAsync(string codigo)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Codigo == codigo);
        }
    }
}