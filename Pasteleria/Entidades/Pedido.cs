using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pasteleria.Entidades
{
   

    public class Pedido
    {
        public Guid Id { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public required string Estado { get; set; } // Pendiente, En Proceso, Completado
        public decimal Total { get; set; }


        public Guid UsuarioId { get; set; }
        public virtual Usuario? Usuario { get; set; }

       
        public virtual IEnumerable<DetallePedido> Detalles { get; set; } = new List<DetallePedido>();
    }

   
}