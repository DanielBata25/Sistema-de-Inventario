using Entity.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public abstract class ADataGenerica<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        protected ADataGenerica(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
    }
}