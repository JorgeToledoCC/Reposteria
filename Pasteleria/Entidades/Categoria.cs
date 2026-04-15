using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pasteleria.Entidades
{
    public class Categoria
    {
        public Guid Id { get; set; }
        public required string Nombre { get; set; }
        public string? Descripcion { get; set; }

      
        public virtual IEnumerable<Producto> Productos { get; set; } = new List<Producto>();
    }

   
}