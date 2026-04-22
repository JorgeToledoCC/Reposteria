namespace Pasteleria.DTO.Pedido;
public class AgregarPedidoOutput()
{
    public DateTime FechaEntrega {get;set;}
    public decimal Total {get;set;}
    public List<ProductosInput> Productos{get;set;} = new List<ProductosInput>();
}

