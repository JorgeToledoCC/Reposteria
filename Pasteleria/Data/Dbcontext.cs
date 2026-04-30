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

            
           
        }
    }
}