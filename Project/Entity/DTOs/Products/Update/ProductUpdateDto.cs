namespace Entity.DTOs.Products
{
    public class ProductUpdateDto
    {
        public string Codigo { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        public decimal Precio { get; set; }

        public int Stock { get; set; }

        public bool Activo { get; set; }
    }
}