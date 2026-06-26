namespace Entity.DTOs.Users
{
    public class UserUpdateDto
    {
        public string Nombre { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Rol { get; set; } = "Viewer";

        public bool Activo { get; set; }
    }
}