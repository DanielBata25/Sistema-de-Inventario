using Entity.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Entity.Infrastructure.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

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

            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Product> Products { get; set; }
    }
}