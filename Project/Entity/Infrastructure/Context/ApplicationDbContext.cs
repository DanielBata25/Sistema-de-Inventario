using Entity.Model;
using Entity.Model.Auth;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Entity.Infrastructure.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Products");

                entity.HasKey(p => p.Id);

                entity.Property(p => p.Codigo)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(p => p.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(p => p.Descripcion)
                    .HasMaxLength(300);

                entity.Property(p => p.Precio)
                    .HasPrecision(18, 2)
                    .IsRequired();

                entity.Property(p => p.Stock)
                    .IsRequired();

                entity.Property(p => p.Activo)
                    .IsRequired();

                entity.Property(p => p.FechaCreacion)
                    .IsRequired();

                entity.Property(p => p.FechaActualizacion)
                    .IsRequired(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");

                entity.HasKey(u => u.Id);

                entity.Property(u => u.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.HasIndex(u => u.Email)
                    .IsUnique();

                entity.Property(u => u.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(u => u.Rol)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(u => u.Activo)
                    .IsRequired();

                entity.Property(u => u.FechaCreacion)
                    .IsRequired();

                entity.Property(u => u.FechaActualizacion)
                    .IsRequired(false);

                entity.HasMany(u => u.RefreshTokens)
                    .WithOne(rt => rt.User)
                    .HasForeignKey(rt => rt.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.ToTable("RefreshTokens");

                entity.HasKey(rt => rt.Id);

                entity.Property(rt => rt.Token)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasIndex(rt => rt.Token)
                    .IsUnique();

                entity.Property(rt => rt.FechaCreacion)
                    .IsRequired();

                entity.Property(rt => rt.FechaExpiracion)
                    .IsRequired();

                entity.Property(rt => rt.Revocado)
                    .IsRequired();

                entity.Property(rt => rt.FechaRevocacion)
                    .IsRequired(false);

                entity.Property(rt => rt.UserId)
                    .IsRequired();
            });

            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}