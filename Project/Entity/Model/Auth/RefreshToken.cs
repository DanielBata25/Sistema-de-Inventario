namespace Entity.Model.Auth
{
    public class RefreshToken
    {
        public int Id { get; set; }

        public string Token { get; set; } = string.Empty;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public DateTime FechaExpiracion { get; set; }

        public bool Revocado { get; set; } = false;

        public DateTime? FechaRevocacion { get; set; }

        public int UserId { get; set; }

        public User User { get; set; } = null!;
    }
}