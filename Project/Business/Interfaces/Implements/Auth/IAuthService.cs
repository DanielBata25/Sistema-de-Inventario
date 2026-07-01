using Entity.DTOs.Auth;

namespace Business.Interfaces.Implements.Auth
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto dto);

        Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto dto);

        Task<bool> LogoutAsync(LogoutDto dto);
    }
}