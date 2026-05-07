using System.Collections.Generic;
using ApiReposteria.Entidades;
using ApiReposteria.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiReposteria.Dtos;
using ApiReposteria.Dtos.Cliente;
using ApiReposteria.Dtos.Direction;

namespace ApiReposteria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DireccionesController : ControllerBase
    {
        private readonly ReposteriaContext _contexto;

        public DireccionesController(ReposteriaContext contexto)
        {
            _contexto = contexto;
        }
        
        [HttpPost]
        public async Task<ActionResult<IEnumerable<GetDirectionOutput>>> GetDirections([FromBody] Guid ClienteId)
        {

            var Directions =(from d in _contexto.Direcciones
            where d.ClienteId == ClienteId
            select d).ToList();
            var salida = new List<GetDirectionOutput>();
            foreach(var dir in Directions)
            {
                salida.Add(new GetDirectionOutput
                {
                    Id = dir.Id,
                    Nombre = dir.Nombre,
                    Ubicacion = dir.Ubicacion
                });
            }
            return Ok(salida);
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchDireccion(Guid id, [FromBody] GetDirectionOutput input)
        {
            var direccion = await _contexto.Direcciones.FindAsync(id);
            if (direccion == null) return NotFound();

            if (!string.IsNullOrEmpty(input.Nombre)) direccion.Nombre = input.Nombre;
            if (!string.IsNullOrEmpty(input.Ubicacion)) direccion.Ubicacion = input.Ubicacion;

            await _contexto.SaveChangesAsync();
            return NoContent();
        }
    }
}
