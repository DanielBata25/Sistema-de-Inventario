namespace Entity.DTOs.Auth
{
    public class AuthResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;

        public DateTime Expiration { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Rol { get; set; } = string.Empty;
    }
}