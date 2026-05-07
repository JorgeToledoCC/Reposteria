namespace ApiReposteria.Dtos.Cliente;
public class AgregarClienteDtoInput(){
    public int CI {get;set;}
    public string? Extension{get;set;}
    public required string Nombre {get;set;}
    public required string Telefono{get;set;}
    public ICollection<DireccionDtoInput> Direcciones{get;set;} = new List<DireccionDtoInput>();
}

public class DireccionDtoInput()
{   
    public required string Nombre {get;set;}
    public required string Ubicacion{get;set;}
}