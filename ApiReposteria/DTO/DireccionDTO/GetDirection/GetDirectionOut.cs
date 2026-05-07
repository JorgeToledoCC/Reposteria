using ApiReposteria.Enum;

namespace ApiReposteria.Dtos.Direction;
public class GetDirectionOutput()
{  
    public Guid Id {get;set;}
    public required string Nombre {get;set;}
    public required string Ubicacion{get;set;}
   
}
    
