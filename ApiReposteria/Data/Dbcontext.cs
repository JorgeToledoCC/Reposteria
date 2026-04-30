using Microsoft.EntityFrameworkCore;
using ApiReposteria.Entidades;

namespace ApiReposteria.Data;

public class ReposteriaContext : DbContext
{
    public ReposteriaContext(DbContextOptions<ReposteriaContext> options) : base(options) { }

    // Tablas (DbSets)
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Direccion> Direcciones { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Producto> Productos { get; set; }
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<DetallePedido> DetallesPedidos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Cliente>().Property(c=>c.Extension).HasMaxLength(2);

        // 1. Configuración de Precisión Decimal (Crucial para dinero)
        modelBuilder.Entity<Producto>()
            .Property(p => p.Precio)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<DetallePedido>()
            .Property(d => d.PrecioUnitario)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Pedido>()
            .Property(p => p.Total)
            .HasColumnType("decimal(18,2)");

        // 2. Relación Categoria -> Productos (1:N)
        modelBuilder.Entity<Producto>()
            .HasOne(p => p.Categoria)
            .WithMany(c => c.Productos)
            .HasForeignKey(p => p.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict); // Evita borrar categorías con productos

        // 3. Relación Cliente -> Direcciones (1:N)
        modelBuilder.Entity<Direccion>()
            .HasOne(d => d.Cliente)
            .WithMany(c => c.Direcciones)
            .HasForeignKey(d => d.ClienteId);

        // 4. Relación Cliente -> Pedidos (1:N)
        modelBuilder.Entity<Pedido>()
            .HasOne(p => p.Cliente)
            .WithMany(c => c.Pedidos)
            .HasForeignKey(p => p.ClienteId);

        // 5. Relación Pedido -> Direccion (Opcional)
        modelBuilder.Entity<Pedido>()
            .HasOne(p => p.Direccion)
            .WithMany() // Una dirección puede estar en varios pedidos
            .HasForeignKey(p => p.DireccionId)
            .OnDelete(DeleteBehavior.NoAction);

        // 6. Configuración de DetallePedido (Relación Muchos a Muchos implícita)
        modelBuilder.Entity<DetallePedido>()
            .HasOne(dp => dp.Pedido)
            .WithMany(p => p.Detalles)
            .HasForeignKey(dp => dp.PedidoId);

        modelBuilder.Entity<DetallePedido>()
            .HasOne(dp => dp.Producto)
            .WithMany(p => p.Detalles)
            .HasForeignKey(dp => dp.ProductoId);
            
        // 7. Seed Data (Datos Iniciales)
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Ejemplo de cómo agregar datos iniciales para que la DB no esté vacía
       // 1. Definir IDs fijos para que no cambien en cada migración
    Guid categoriaPastelesId = Guid.Parse("d28c0c1b-25bc-48c1-90a2-9b2f6b39d1b1");
    Guid productoId = Guid.Parse("a123bc45-1234-4321-8765-abcdef123456");

    // 2. Sembrar Categoría
    modelBuilder.Entity<Categoria>().HasData(new Categoria 
    { 
        Id = categoriaPastelesId, 
        Nombre = "Pasteles" 
    });

    // 3. Sembrar Producto 
    // Nota: Usamos un objeto anónimo o nos aseguramos de llenar solo los campos de la tabla
    // Para evitar errores con 'required Categoria', EF Core permite pasar el ID directamente:
    modelBuilder.Entity<Producto>().HasData(new
    {
        Id = productoId,
        Nombre = "Tarta de Queso",
        Precio = 15.00m,
        CategoriaId = categoriaPastelesId, // Relacionamos por ID
        Descripcion = "Tarta cremosa con base de galleta",
        Stock = 4
        
    });
    }
}