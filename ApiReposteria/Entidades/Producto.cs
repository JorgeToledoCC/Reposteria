namespace ApiReposteria.Entidades;

public class Producto()
{
    public Guid Id {get;set;}
    public required string Nombre {get;set;}
    public string? Descripcion{get;set;}
    public required decimal Precio{get;set;}
    public string? ImgUrl{get;set;}
    
    public int Stock {get;set;}
    
    public Guid CategoriaId { get; set; }
    public  required Categoria Categoria { get; set; }
    public ICollection<DetallePedido> Detalles{get;set;} = new List<DetallePedido>();
}