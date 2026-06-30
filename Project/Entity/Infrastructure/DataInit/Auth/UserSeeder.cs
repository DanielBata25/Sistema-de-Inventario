using Entity.Infrastructure.Context;
using Entity.Model.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Entity.Infrastructure.DataInit.Auth
{
    public static class UserSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (await context.Users.AnyAsync())
            {
                return;
            }

            var passwordHasher = new PasswordHasher<User>();

            var adminUser = new User
            {
                Nombre = "Administrador",
                Email = "admin@empresa.com",
                Rol = "Admin",
                Activo = true,
                FechaCreacion = DateTime.Now
            };

            adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "Admin123*");

            await context.Users.AddAsync(adminUser);
            await context.SaveChangesAsync();
        }
    }
}