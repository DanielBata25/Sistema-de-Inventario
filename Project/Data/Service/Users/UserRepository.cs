using Data.Interfaces.Implements.Users;
using Data.Repository;
using Entity.Infrastructure.Context;
using Entity.Model.Auth;
using Microsoft.EntityFrameworkCore;

namespace Data.Service.Users
{
    public class UserRepository : DataGeneric<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
            
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users
                .AnyAsync(u => u.Email == email);
        }
    }
}