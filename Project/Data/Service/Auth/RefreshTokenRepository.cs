using Data.Interfaces.Implements.Auth;
using Data.Repository;
using Entity.Infrastructure.Context;
using Entity.Model.Auth;
using Microsoft.EntityFrameworkCore;

namespace Data.Service.Auth
{
    public class RefreshTokenRepository : DataGeneric<RefreshToken>, IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _context;

        public RefreshTokenRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == token);
        }

        public async Task<bool> RevokeAsync(string token)
        {
            var refreshToken = await GetByTokenAsync(token);

            if (refreshToken == null)
            {
                return false;
            }

            refreshToken.Revocado = true;
            refreshToken.FechaRevocacion = DateTime.Now;

            _context.RefreshTokens.Update(refreshToken);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}