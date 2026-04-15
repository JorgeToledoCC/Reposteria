using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pasteleria.Entidades
{
    

    public class PastelPersonalizado
    {
        public Guid Id { get; set; }
        public required string SaborBizcocho { get; set; }
        public required string Relleno { get; set; }
        public required string Cobertura { get; set; }
        public int Pisos { get; set; }
        public string? Dedicatoria { get; set; }
        public string? NotasAdicionales { get; set; }
        
      
        public virtual DetallePedido? DetallePedido { get; set; }
    }
}