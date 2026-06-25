using Entity.Infrastructure.Context;
using Entity.Infrastructure.DataInit.Products;

namespace Entity.Infrastructure.DataInit
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            await ProductSeeder.SeedAsync(context);
        }
    }
}