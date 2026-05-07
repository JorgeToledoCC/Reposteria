using System.Collections.Generic;
using ApiReposteria.Entidades;
using ApiReposteria.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiReposteria.Dtos;
using ApiReposteria.Dtos.Cliente;

namespace ApiReposteria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly ReposteriaContext _contexto;

        public ClienteController(ReposteriaContext contexto)
        {
            _contexto = contexto;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            var Clientes = await _contexto.Clientes.Include(c => c.Direcciones).ToArrayAsync();
            return Ok(Clientes);
        }

        [HttpPost]
        public async Task<ActionResult<Cliente>> CreateCliente([FromBody] AgregarClienteDtoInput ClienteIn)
        {
            var existe = await _contexto.Clientes
                .AnyAsync(c => c.CI == ClienteIn.CI && c.Extension == ClienteIn.Extension);

            if (existe)
            {
                return BadRequest($"Ya existe un cliente registrado con el CI {ClienteIn.CI} y extensión {ClienteIn.Extension}.");
            }

            Cliente cliente = new Cliente
            {
                Id = Guid.NewGuid(), 
                CI = ClienteIn.CI,
                Extension = ClienteIn.Extension,
                Nombre = ClienteIn.Nombre,
                Telefono = ClienteIn.Telefono,
                Direcciones = new List<Direccion>() 
            };

            foreach (var dir in ClienteIn.Direcciones)
            {
                Direccion direccion = new Direccion
                {
                    Id = Guid.NewGuid(),
                    Nombre = dir.Nombre,
                    Ubicacion = dir.Ubicacion,
                    Cliente = cliente
                };
                cliente.Direcciones.Add(direccion);
            }

            _contexto.Clientes.Add(cliente);
            await _contexto.SaveChangesAsync();

            return CreatedAtAction(nameof(GetClientes), new { id = cliente.Id }, cliente);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchCliente(Guid id, [FromBody] AgregarClienteDtoInput input)
{
            var cliente = await _contexto.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();

            if (input.CI != 0 || !string.IsNullOrWhiteSpace(input.Extension))
            {
                int nuevoCI = input.CI != 0 ? input.CI : cliente.CI;
                string? nuevaExt = !string.IsNullOrWhiteSpace(input.Extension) ? input.Extension : cliente.Extension;

                var duplicado = await _contexto.Clientes
                    .AnyAsync(c => c.Id != id && c.CI == nuevoCI && c.Extension == nuevaExt);

                if (duplicado)
                {
                    return BadRequest($"No se puede actualizar: ya existe otro cliente con el CI {nuevoCI} y extensión {nuevaExt}.");
                }

                cliente.CI = nuevoCI;
                cliente.Extension = nuevaExt ?? cliente.Extension;
    }       

                if (!string.IsNullOrWhiteSpace(input.Nombre)) cliente.Nombre = input.Nombre;
                if (!string.IsNullOrWhiteSpace(input.Telefono)) cliente.Telefono = input.Telefono;

                await _contexto.SaveChangesAsync();

                return NoContent();
}
    }
}