using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pasteleria.DTO.Pedido;
using Pasteleria.Entidades;
using Pasteleria.Infraestructura;

namespace Pasteleria.Controlllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        //dependencia de inyeccion 
        // no se inyectan a traves de  new
        // se inyectan a traves de los servicios 
        // saas, 
        private readonly PasteleriaContext _contexto;

        public PedidoController(PasteleriaContext contexto){
            _contexto = contexto;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<Pedido>>> GetPedido()
        {
            // Agregamos Include para que traiga los detalles del pedido
            var Pedidos = await _contexto.Pedidos.ToListAsync();
            return Ok(Pedidos);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Pedido>> GetPedidoid(Guid id){
            // Cambiamos FindAsync por FirstOrDefaultAsync para poder encadenar el Include
            var Pedido = await _contexto.Pedidos.FirstOrDefaultAsync(p => p.Id == id);
            if(Pedido == null)
            {
                return NotFound();
            }
            return Ok(Pedido);
        }

        [HttpPost]
        public async Task<ActionResult<AgregarPedidoOutput>> CrearPedido([FromBody] PedidoInput pedidoInput)
        {
            // 1. Creamos la instancia real del Pedido a guardar
            var nuevoPedido = new Pedido
            {
                Id = Guid.NewGuid(),
                UsuarioId = pedidoInput.UsuarioId,
                Fecha = pedidoInput.FechaEntrega, // Usamos la fecha que viene del Input
                Estado = "Pendiente",
                Total = 0,
                Detalles = new List<DetallePedido>()
            };

            // Casteamos a List para poder hacer .Add()
            var listaDetalles = (List<DetallePedido>)nuevoPedido.Detalles;

            foreach(ProductosInput p in pedidoInput.Productos)
            {   
                // 2. Buscamos el producto por Nombre (Find asume que es el ID, por eso usamos FirstOrDefault)
                var product = await _contexto.Productos.FirstOrDefaultAsync(x => x.Nombre == p.Nombre);
                
                if (product != null && product.Stock >= p.Cantidad)
                {
                    
                    product.Stock -= p.Cantidad;
                    nuevoPedido.Total += (product.Precio * p.Cantidad); 
                    
                    var detalle = new DetallePedido
                    {
                        Id = Guid.NewGuid(),
                        Cantidad = p.Cantidad,
                        PrecioUnitario = product.Precio, // Tomamos el precio actual del producto
                        ProductoId = product.Id, // Enlazamos con el ID del producto encontrado
                        
                        
                    };
                    
                    listaDetalles.Add(detalle);
                    
                }
                else
                {
                    return BadRequest($"El producto {p.Nombre} no existe o no tiene stock suficiente para la cantidad pedida.");
                }
            }
           
            // 6. Agregamos el nuevo pedido al DbContext y guardamos cambios
            _contexto.Pedidos.Add(nuevoPedido);
            await _contexto.SaveChangesAsync();
            
            var pedidosalida = new AgregarPedidoOutput
            {
                Total = nuevoPedido.Total,
                FechaEntrega = nuevoPedido.Fecha,
                Productos = pedidoInput.Productos

            };

            return Ok(pedidosalida);
        }

        //pa borrar
        [HttpDelete]
        public async Task<ActionResult<Pedido>> DeletePadido(Guid id)
        {
            var pedido = await _contexto.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }
            pedido.Estado="Cancelado";
            await _contexto.SaveChangesAsync();
            return Ok();
        }
    }
}