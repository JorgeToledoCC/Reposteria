using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiReposteria.Data;
using ApiReposteria.Dtos.Pedido;
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

        [HttpPost("Pendientes")]
        public async Task<ActionResult<List<GetPedidoOutput>>> GetPedidosPendientes([FromBody] Guid ClienteId)
        {
            var cliente = _context.Clientes.FirstOrDefault(c => c.Id == ClienteId);
            if (cliente == null) return BadRequest("cliente no encontrado");

            var ListaPedidosSalida = new List<GetPedidoOutput>();
            var ListaPedidos = _context.Pedidos.Where(p => p.ClienteId == cliente.Id).ToList();
            var direcciones = _context.Direcciones.Where(d => d.ClienteId == cliente.Id).ToList();

            foreach (Pedido p in ListaPedidos)
            {
                if (p.EstadoPedido == Enum.EstadoPedido.Cancelado || p.EstadoPedido == Enum.EstadoPedido.Entregado) continue;

                string dnombre = "";
                string dubicacion = "";
                if (p.Entrega == Enum.TipoEntrega.Domicilio)
                {
                    var dir = direcciones.Find(d => p.DireccionId == d.Id);
                    if (dir != null) { dnombre = dir.Nombre; dubicacion = dir.Ubicacion; }
                }

                ListaPedidosSalida.Add(new GetPedidoOutput
                {
                    Id = p.Id,
                    FechaEntrega = p.FechaEntrega,
                    EstadoPedido = p.EstadoPedido.ToString(),
                    Total = p.Total,
                    Entrega = p.Entrega.ToString(),
                    DireccionNombre = dnombre,
                    Direccion = dubicacion
                });
            }
            return Ok(ListaPedidosSalida);
        }

        [HttpPost]
        public async Task<ActionResult<GetPedidoOutput>> CrearPedido([FromBody] AgregarPedidoInput pedidoInput)
        {
            var cliente = await _context.Clientes.FindAsync(pedidoInput.ClienteId);
            if (cliente == null) return NotFound("el cliente no existe");

            Pedido pedido = new Pedido
            {
                Id = Guid.NewGuid(),
                Cliente = cliente,
                FechaPedido = DateTime.Now,
                FechaEntrega = pedidoInput.FechaEntrega,
                EstadoPedido = Enum.EstadoPedido.Pendiente,
                Entrega = pedidoInput.Entrega,
                Total = 0
            };

            if (pedido.Entrega == Enum.TipoEntrega.Domicilio)
            {
                if (pedidoInput.DireccionId == null) return BadRequest("el pedido es a domicilio y no se seleciono una direccion");
                pedido.DireccionId = pedidoInput.DireccionId;
            }

            decimal Total = 0;
            List<DetallePedido> detallePedidos = new List<DetallePedido>();

            foreach (var dp in pedidoInput.Detalles)
            {
                Producto? producto = _context.Productos.FirstOrDefault(p => p.Nombre == dp.NombreProducto);
                
                if (producto == null) return BadRequest($"producto '{dp.NombreProducto}' no valido");

                // --- AQUÍ BAJAMOS EL STOCK SIEMPRE ---
                if (producto.Stock < dp.Cantidad)
                {
                    return BadRequest($"No hay suficiente stock de {producto.Nombre}. Quedan: {producto.Stock}");
                }

                producto.Stock -= dp.Cantidad; // Restamos del inventario
                Total += producto.Precio * dp.Cantidad;

                detallePedidos.Add(new DetallePedido
                {
                    Pedido = pedido,
                    ProductoId = producto.Id,
                    Producto = producto,
                    Cantidad = dp.Cantidad,
                    PrecioUnitario = producto.Precio
                });
            }

            pedido.Total = Total;

            if (pedido.FechaEntrega.Date == DateTime.Today.Date)
            {
                pedido.EstadoPedido = Enum.EstadoPedido.En_Preparacion;
            }

            _context.Pedidos.Add(pedido);
            _context.DetallesPedidos.AddRange(detallePedidos);
            await _context.SaveChangesAsync();

            // Lógica de salida para Swagger
            string resNombre = "";
            string resUbi = "";
            if (pedido.Entrega == Enum.TipoEntrega.Domicilio)
            {
                var dir = _context.Direcciones.FirstOrDefault(d => d.Id == pedidoInput.DireccionId);
                if (dir != null) { resNombre = dir.Nombre; resUbi = dir.Ubicacion; }
            }

            var salida = new GetPedidoOutput
            {
                Id = pedido.Id,
                FechaEntrega = pedido.FechaEntrega,
                EstadoPedido = pedido.EstadoPedido.ToString(),
                Total = pedido.Total,
                Entrega = pedido.Entrega.ToString(),
                DireccionNombre = resNombre,
                Direccion = resUbi
            };

            return CreatedAtAction(nameof(CrearPedido), new { id = pedido.Id }, salida);
        }

        [HttpPatch("cancelar")]
        public async Task<ActionResult> CancelarPedido([FromBody] Guid IDPedido)
        {
            var pedido = await _context.Pedidos.FindAsync(IDPedido);
            if (pedido == null) return BadRequest("no existe ese pedido");

            if (pedido.EstadoPedido == Enum.EstadoPedido.Pendiente)
            {
                pedido.EstadoPedido = Enum.EstadoPedido.Cancelado;
                await _context.SaveChangesAsync();
                return Ok("pedido cancelado");
            }
            return BadRequest("el pedido ya se encuantra en preparacion");
        }
    }
}