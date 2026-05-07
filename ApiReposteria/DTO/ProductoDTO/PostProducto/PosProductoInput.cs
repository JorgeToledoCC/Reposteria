namespace ApiReposteria.Dtos.Producto;
public class AgregarProductoInput()
{
    public required string Nombre {get;set;}
    public string? Descripcion{get;set;}
    public decimal Precio{get;set;}
    public string? ImgUrl{get;set;}
    public int stock{get;set;}

    public required string NombreCategoria { get; set; }
    
}