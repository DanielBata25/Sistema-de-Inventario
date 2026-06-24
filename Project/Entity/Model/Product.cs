using Entity.Model.Base;

namespace Entity.Model
{
    public class Product : ProductBase
    {
        public int Id { get; set; }

        public bool Activo { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public DateTime? FechaActualizacion { get; set; }
    }
}