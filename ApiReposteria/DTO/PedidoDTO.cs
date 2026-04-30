using ApiReposteria.Enum;

namespace ApiReposteria.Dtos;

public record AgregarPedidoInput()
{




    public DateTime FechaEntrega { get; set; }
    
    public TipoEntrega Entrega { get; set; } 

    public Guid? DireccionId { get; set; }

    public Guid ClienteId { get; set; }


    public List<AgregarDetallePedido> Detalles { get; set; } = new List<AgregarDetallePedido>();



};

public class AgregarDetallePedido()
{
    public required string NombreProducto{get;set;}
    //public Guid ProductoId{get;set;}
    public int Cantidad{get;set;}= 1;
}
