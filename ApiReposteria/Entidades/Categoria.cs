namespace ApiReposteria.Entidades;

public class Categoria
{
    public Guid Id { get; set; }
    public required string Nombre { get; set; } // Ejemplo: Tortas, Bocaditos, Panadería
    public string? Descripcion { get; set; }

    // Relación: Una categoría tiene muchos productos
    public ICollection<Producto> Productos { get; set; } = new List<Producto>();
}