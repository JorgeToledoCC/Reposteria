using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pasteleria.Entidades
{
   

    public class Usuario
    {
        public Guid Id { get; set; }
        public required string Nombre { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Rol { get; set; } // "Admin" o "Cliente"

        
        public virtual IEnumerable<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }

    
}