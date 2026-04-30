using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pasteleria.Entidades
{
   

    public class Producto
    {
        public Guid Id { get; set; }
        public required string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }

        
        public Guid CategoriaId { get; set; }
        public virtual Categoria? Categoria { get; set; }
    }

    
}