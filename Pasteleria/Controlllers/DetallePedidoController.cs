using System.Collections;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pasteleria.Entidades;
using Pasteleria.Infraestructura;

namespace Pasteleria.Controlllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetallePedidoController : ControllerBase
    {
        private readonly PasteleriaContext _contexto;

        public DetallePedidoController(PasteleriaContext contexto){
            _contexto = contexto;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<DetallePedido>>> GetDetallePedido()
        {
            var DetallesP = await _contexto.DetallesPedidos.ToListAsync();
            return Ok(DetallesP);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DetallePedido>> GetDetallePedidoid(Guid id){
            var DetallesP = await _contexto.DetallesPedidos.FindAsync(id);
            if(DetallesP == null)
            {
                return NotFound();
            }
            return Ok(DetallesP);
        }

        [HttpPost]
        public async Task<ActionResult<DetallePedido>> CrearCliet([FromBody] DetallePedido DetalleP)
        {
            _contexto.DetallesPedidos.Add(DetalleP);
            await _contexto.SaveChangesAsync();
            return Ok(DetalleP);
        }



    }
}
