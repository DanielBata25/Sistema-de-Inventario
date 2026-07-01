using Data.Interfaces.IRepository;
using Entity.Model.Auth;

namespace Data.Interfaces.Implements.Auth
{
    public interface IRefreshTokenRepository : IDataGeneric<RefreshToken>
    {
        Task<RefreshToken?> GetByTokenAsync(string token);

        Task<bool> RevokeAsync(string token);
    }
}