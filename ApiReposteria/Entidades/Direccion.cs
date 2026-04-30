namespace ApiReposteria.Entidades;

public class Direccion()
{
    public Guid Id {get;set;}
    
    public required string Nombre {get;set;}
    public required string Ubicacion{get;set;}
    
    public Guid ClienteId {get;set;}
    public required Cliente Cliente {get;set;}
}