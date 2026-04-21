using System.Collections;
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
            var Pedidos = await _contexto.Pedidos.ToListAsync();
            return Ok(Pedidos);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Pedido>> GetPedidoid(Guid id){
            var Pedido = await _contexto.Pedidos.FindAsync(id);
            if(Pedido == null)
            {
                return NotFound();
            }
            return Ok(Pedido);
        }

        [HttpPost]
        public async Task<ActionResult<Pedido>> CrearPedido([FromBody] PedidoInput pedido)
        {
            List<ProductosInput> productos = pedido.Productos;
            foreach(ProductosInput p in productos)
            {   
              var product =   _contexto.Productos.Find(p.Nombre);
                if (product !=null)
                {
              product.Stock -= p.Cantidad;
                    
                }

            }
           
            await _contexto.SaveChangesAsync();
            return Ok(pedido);
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
