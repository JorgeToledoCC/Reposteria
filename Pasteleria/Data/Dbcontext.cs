using Microsoft.EntityFrameworkCore;
using Pasteleria.Entidades;

namespace Pasteleria.Infraestructura
{
    public class PasteleriaContext : DbContext
    {
        public PasteleriaContext(DbContextOptions<PasteleriaContext> options) : base(options) { }

        // Tablas
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<DetallePedido> DetallesPedidos { get; set; }
        public DbSet<PastelPersonalizado> PastelesPersonalizados { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<Producto>()
                .Property(p => p.Precio)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Pedido>()
                .Property(p => p.Total)
                .HasPrecision(18, 2);

            modelBuilder.Entity<DetallePedido>()
                .Property(d => d.PrecioUnitario)
                .HasPrecision(18, 2);

            // Configuración de Relaciones

            
            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Categoria)
                .WithMany(c => c.Productos)
                .HasForeignKey(p => p.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

           
            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Usuario)
                .WithMany(u => u.Pedidos)
                .HasForeignKey(p => p.UsuarioId);

           
            modelBuilder.Entity<DetallePedido>()
                .HasOne(d => d.Pedido)
                .WithMany(p => p.Detalles)
                .HasForeignKey(d => d.PedidoId);

           
            modelBuilder.Entity<DetallePedido>()
                .HasOne(d => d.PastelPersonalizado)
                .WithOne(p => p.DetallePedido)
                .HasForeignKey<DetallePedido>(d => d.PastelPersonalizadoId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

           
            modelBuilder.Entity<DetallePedido>()
                .HasOne(d => d.Producto)
                .WithMany() 
                .HasForeignKey(d => d.ProductoId)
                .IsRequired(false);
        }
    }
}