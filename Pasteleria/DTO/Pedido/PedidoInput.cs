namespace Pasteleria.DTO.Pedido;
public class PedidoInput()
{
    public DateTime FechaEntrega {get;set;}
    public Guid UsuarioId{get;set;}
    public List<ProductosInput> Productos{get;set;} = new List<ProductosInput>();
}

public class ProductosInput()
{
    public required string Nombre{get;set;}
    public required int Cantidad{get;set;}

}