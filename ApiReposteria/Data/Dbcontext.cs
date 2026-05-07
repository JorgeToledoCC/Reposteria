using Microsoft.EntityFrameworkCore;
using ApiReposteria.Entidades;

namespace ApiReposteria.Data;

public class ReposteriaContext : DbContext
{
    public ReposteriaContext(DbContextOptions<ReposteriaContext> options) : base(options) { }

    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Direccion> Direcciones { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Producto> Productos { get; set; }
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<DetallePedido> DetallesPedidos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // --- NUEVA REGLA DE SEGURIDAD ---
        modelBuilder.Entity<Cliente>()
            .HasIndex(c => new { c.CI, c.Extension })
            .IsUnique();

        modelBuilder.Entity<Cliente>().Property(c => c.Extension).HasMaxLength(2);

        modelBuilder.Entity<Producto>()
            .Property(p => p.Precio)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<DetallePedido>()
            .Property(d => d.PrecioUnitario)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Pedido>()
            .Property(p => p.Total)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Producto>()
            .HasOne(p => p.Categoria)
            .WithMany(c => c.Productos)
            .HasForeignKey(p => p.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Direccion>()
            .HasOne(d => d.Cliente)
            .WithMany(c => c.Direcciones)
            .HasForeignKey(d => d.ClienteId);

        modelBuilder.Entity<Pedido>()
            .HasOne(p => p.Cliente)
            .WithMany(c => c.Pedidos)
            .HasForeignKey(p => p.ClienteId);

        modelBuilder.Entity<Pedido>()
            .HasOne(p => p.Direccion)
            .WithMany()
            .HasForeignKey(p => p.DireccionId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<DetallePedido>()
            .HasOne(dp => dp.Pedido)
            .WithMany(p => p.Detalles)
            .HasForeignKey(dp => dp.PedidoId);

        modelBuilder.Entity<DetallePedido>()
            .HasOne(dp => dp.Producto)
            .WithMany(p => p.Detalles)
            .HasForeignKey(dp => dp.ProductoId);
            
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        Guid catPastelesId = Guid.Parse("d28c0c1b-25bc-48c1-90a2-9b2f6b39d1b1");
        Guid catPostresId = Guid.Parse("f47ac10b-58cc-4372-a567-0e02b2c3d4e5");
        Guid catSaladosId = Guid.Parse("722f1ad1-a4aa-4ac4-86dc-984362a98f7e");
        Guid catBebidasId = Guid.Parse("b11d2e3f-4a5b-6c7d-8e9f-0a1b2c3d4e5f");

        Guid productoTartaId = Guid.Parse("a123bc45-1234-4321-8765-abcdef123456");

        modelBuilder.Entity<Categoria>().HasData(
            new Categoria { Id = catPastelesId, Nombre = "Pasteles" },
            new Categoria { Id = catPostresId, Nombre = "Postres" },
            new Categoria { Id = catSaladosId, Nombre = "Salados" },
            new Categoria { Id = catBebidasId, Nombre = "Bebidas" }
        );

        modelBuilder.Entity<Producto>().HasData(new
        {
            Id = productoTartaId,
            Nombre = "Tarta de Queso",
            Precio = 15.00m,
            CategoriaId = catPastelesId,
            Descripcion = "Tarta cremosa con base de galleta",
            Stock = 4
        });
    }
}