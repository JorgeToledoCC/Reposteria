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
    public class PastelesPersonalizadosController : ControllerBase
    {
        private readonly PasteleriaContext _contexto;

        public PastelesPersonalizadosController(PasteleriaContext contexto){
            _contexto = contexto;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<PastelPersonalizado>>> GetPasteles()
        {
            var pasteles = await _contexto.PastelesPersonalizados.ToListAsync();
            return Ok(pasteles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PastelPersonalizado>> GetPastelid(Guid id){
            var Pastel = await _contexto.PastelesPersonalizados.FindAsync(id);
            if(Pastel == null)
            {
                return NotFound();
            }
            return Ok(Pastel);
        }

        [HttpPost]
        public async Task<ActionResult<PastelPersonalizado>> CrearCliet([FromBody] PastelPersonalizado Pastel)
        {
            _contexto.PastelesPersonalizados.Add(Pastel);
            await _contexto.SaveChangesAsync();
            return Ok(Pastel);
        }

        [HttpDelete]
        public async Task<ActionResult<PastelPersonalizado>> EliminarPastel(Guid id)
        {
            var Pastel = await _contexto.PastelesPersonalizados.FindAsync(id);
            if (Pastel == null)
            {
                return NotFound();
            }
            _contexto.PastelesPersonalizados.Remove(Pastel);
            await _contexto.SaveChangesAsync();
            return Ok(Pastel);
        }
        


    }
}
