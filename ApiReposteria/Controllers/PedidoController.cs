using ApiReposteria.Data;
using ApiReposteria.Dtos;
using ApiReposteria.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiReposteria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly ReposteriaContext _context;
        public PedidoController(ReposteriaContext context) => _context = context;
        [HttpGet]
        public async Task<ActionResult<Pedido>> GetPedidos()
        {

          return Ok(_context.Pedidos);     
        }
        [HttpPost]
        public async Task<ActionResult<Pedido>> CrearPedido([FromBody] AgregarPedidoInput pedidoInput)
        {
          var Detalles =  pedidoInput.Detalles ;
            /*Pedido pedido = new Pedido
            {
              Id = Guid.NewGuid(),
              ClienteId = _context.Clientes.Find(),

            };*/
            foreach (var pe in _context.Pedidos)
            {
             // _context.DetallesPedidos.Add(new DetallePedido
              //{
            //     
              //});
            };
            return Ok();
        }
    }
}