using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pasteleria.Entidades
{
    

    public class DetallePedido
    {
        public Guid Id { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public Guid PedidoId { get; set; }
        public virtual Pedido? Pedido { get; set; }

  
        public Guid? ProductoId { get; set; }
        public virtual Producto? Producto { get; set; }

       
        public Guid? PastelPersonalizadoId { get; set; }
        public virtual PastelPersonalizado? PastelPersonalizado { get; set; }
    }

    
}