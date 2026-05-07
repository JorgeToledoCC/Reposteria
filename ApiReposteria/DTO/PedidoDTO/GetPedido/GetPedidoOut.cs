using ApiReposteria.Enum;

namespace ApiReposteria.Dtos.Pedido;
public class GetPedidoOutput()
{
    public Guid Id {get;set;}
    public DateTime FechaEntrega{get;set;}
    public required string EstadoPedido{get;set; }
    public decimal Total { get; set; }
    public required string Entrega { get; set; } // ¿Se envía o se recoge?
    public string? DireccionNombre{get;set;}
    public string? Direccion{get;set;}
}