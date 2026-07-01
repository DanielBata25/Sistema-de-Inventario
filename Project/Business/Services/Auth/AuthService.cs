using Business.Interfaces.Implements.Auth;
using Data.Interfaces.Implements.Auth;
using Data.Interfaces.Implements.Users;
using Entity.DTOs.Auth;
using Entity.Model.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Business.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<User> _passwordHasher;

        public AuthService(
            IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _configuration = configuration;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto), "Los datos de inicio de sesión son obligatorios.");
            }

            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                throw new ArgumentException("El correo electrónico es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(dto.Password))
            {
                throw new ArgumentException("La contraseña es obligatoria.");
            }

            var user = await _userRepository.GetByEmailAsync(dto.Email.Trim());

            if (user == null || !user.Activo)
            {
                throw new UnauthorizedAccessException("Credenciales inválidas.");
            }

            var passwordResult = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                dto.Password
            );

            if (passwordResult == PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedAccessException("Credenciales inválidas.");
            }

            return await GenerateAuthResponseAsync(user);
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.RefreshToken))
            {
                throw new ArgumentException("El refresh token es obligatorio.");
            }

            var refreshToken = await _refreshTokenRepository.GetByTokenAsync(dto.RefreshToken.Trim());

            if (refreshToken == null)
            {
                throw new UnauthorizedAccessException("Refresh token inválido.");
            }

            if (refreshToken.Revocado)
            {
                throw new UnauthorizedAccessException("El refresh token ya fue revocado.");
            }

            if (refreshToken.FechaExpiracion < DateTime.Now)
            {
                throw new UnauthorizedAccessException("El refresh token expiró.");
            }

            if (refreshToken.User == null || !refreshToken.User.Activo)
            {
                throw new UnauthorizedAccessException("Usuario no autorizado.");
            }

            await _refreshTokenRepository.RevokeAsync(refreshToken.Token);

            return await GenerateAuthResponseAsync(refreshToken.User);
        }

        public async Task<bool> LogoutAsync(LogoutDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.RefreshToken))
            {
                throw new ArgumentException("El refresh token es obligatorio.");
            }

            return await _refreshTokenRepository.RevokeAsync(dto.RefreshToken.Trim());
        }

        private async Task<AuthResponseDto> GenerateAuthResponseAsync(User user)
        {
            var accessTokenExpiration = DateTime.Now.AddMinutes(GetAccessTokenExpirationMinutes());
            var accessToken = GenerateAccessToken(user, accessTokenExpiration);
            var refreshTokenValue = GenerateRefreshToken();

            var refreshToken = new RefreshToken
            {
                Token = refreshTokenValue,
                UserId = user.Id,
                FechaCreacion = DateTime.Now,
                FechaExpiracion = DateTime.Now.AddDays(GetRefreshTokenExpirationDays()),
                Revocado = false
            };

            await _refreshTokenRepository.AddAsync(refreshToken);

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshTokenValue,
                Expiration = accessTokenExpiration,
                Nombre = user.Nombre,
                Email = user.Email,
                Rol = user.Rol
            };
        }

        private string GenerateAccessToken(User user, DateTime expiration)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Nombre),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Rol)
            };

            var jwtKey = GetJwtKey();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: GetJwtIssuer(),
                audience: GetJwtAudience(),
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];

            using var randomNumberGenerator = RandomNumberGenerator.Create();

            randomNumberGenerator.GetBytes(randomBytes);

            return Convert.ToBase64String(randomBytes);
        }

        private string GetJwtKey()
        {
            return _configuration["Jwt:Key"]
                ?? "SistemaInventarioDevelopmentSecretKey123456789*";
        }

        private string GetJwtIssuer()
        {
            return _configuration["Jwt:Issuer"]
                ?? "SistemaInventario";
        }

        private string GetJwtAudience()
        {
            return _configuration["Jwt:Audience"]
                ?? "SistemaInventarioUsers";
        }

        private int GetAccessTokenExpirationMinutes()
        {
            var value = _configuration["Jwt:ExpirationMinutes"];

            return int.TryParse(value, out var minutes) ? minutes : 60;
        }

        private int GetRefreshTokenExpirationDays()
        {
            var value = _configuration["Jwt:RefreshTokenExpirationDays"];

            return int.TryParse(value, out var days) ? days : 7;
        }
    }
}