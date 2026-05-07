namespace ApiReposteria.Entidades;

public class Cliente()
{
    public Guid Id {get;set;}
    public int CI {get;set;}
    public string? Extension{get;set;}
    public required string Nombre {get;set;}
    public required string Telefono{get;set;}
    public ICollection<Direccion> Direcciones{get;set;} = new List<Direccion>();
    public ICollection<Pedido> Pedidos{get;set;} = new List<Pedido>();

}