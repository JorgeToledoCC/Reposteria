namespace ApiReposteria.Dtos;
public class AgregarProductoInput()
{
    public required string Nombre {get;set;}
    public string? Descripcion{get;set;}
    public decimal Precio{get;set;}
    public string? ImgUrl{get;set;}
    public int stock{get;set;}

    public required string NombreCategoria { get; set; }
    
}

public class AgregarProductoOutput()
{
    public Guid Id{get;set;} 
    public required string Nombre {get;set;}
    public string? Descripcion{get;set;}
    public decimal Precio{get;set;}
    public int stock{get;set;}
    public required string NombreCategoria{get;set;}
}

/*public record PedidoDto(
    Guid Id,
    DateTime FechaPedido,
    string Estado,
    decimal Total,
    int CantidadProductos
);

public record PedidoDetalleDto(
    Guid Id,
    DateTime FechaEntrega,
    string Estado,
    string TipoEntrega,
    string? DireccionEntrega,
    decimal Total,
    List<DetalleItemDto> Items
);

public record DetalleItemDto(
    string ProductoNombre,
    int Cantidad,
    decimal PrecioUnitario,
    decimal Subtotal
);*/