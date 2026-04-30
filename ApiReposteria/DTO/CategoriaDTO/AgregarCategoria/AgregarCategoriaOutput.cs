namespace ApiReposteria.Dtos.Categoria;
public class AgregarCategoriaOutput()
{
    public Guid Id{get;set;}
    public required string Nombre { get; set; } 
    public string? Descripcion { get; set; }    
}