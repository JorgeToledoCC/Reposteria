using ApiReposteria.Enum;

namespace ApiReposteria.Entidades;


public class Pedido()
{
    public Guid Id {get;set;}
    public DateTime FechaPedido{get;set;} = DateTime.Now;
    public DateTime FechaEntrega{get;set;}
    public EstadoPedido EstadoPedido{get;set; }
    public decimal Total { get; set; }
    public TipoEntrega Entrega { get; set; } // ¿Se envía o se recoge?

    public Guid? DireccionId{get;set;}
    public Direccion? Direccion{get;set;}

    public Guid ClienteId {get;set;}
    public required Cliente Cliente{get;set;} 
    
    public ICollection<DetallePedido> Detalles{get;set;} = new List<DetallePedido>();
}