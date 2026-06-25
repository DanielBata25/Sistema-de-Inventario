using Entity.Infrastructure.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;

namespace Entity.Infrastructure.DataInit.Products
{
    public static class ProductSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (await context.Products.AnyAsync())
            {
                return;
            }

            var products = new List<Product>
            {
                new Product
                {
                    Codigo = "PROD-001",
                    Nombre = "Teclado",
                    Descripcion = "Teclado básico para computador",
                    Precio = 45000,
                    Stock = 10,
                    Activo = true,
                    FechaCreacion = DateTime.Now
                },
                new Product
                {
                    Codigo = "PROD-002",
                    Nombre = "Mouse",
                    Descripcion = "Mouse óptico USB",
                    Precio = 25000,
                    Stock = 20,
                    Activo = true,
                    FechaCreacion = DateTime.Now
                },
                new Product
                {
                    Codigo = "PROD-003",
                    Nombre = "Monitor",
                    Descripcion = "Monitor LED 24 pulgadas",
                    Precio = 520000,
                    Stock = 5,
                    Activo = true,
                    FechaCreacion = DateTime.Now
                }
            };

            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
        }
    }
}