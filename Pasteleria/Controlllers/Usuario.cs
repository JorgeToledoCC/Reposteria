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
    public class UsuarioController : ControllerBase
    {
        private readonly PasteleriaContext _contexto;

        public UsuarioController(PasteleriaContext contexto){
            _contexto = contexto;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<Pedido>>> GetPedido()
        {
            var Usuarios = await _contexto.Usuarios.ToListAsync();
            return Ok(Usuarios);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> Getuserid(Guid id){
            var usuario = await _contexto.Usuarios.FindAsync(id);
            if(usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }

        [HttpPost]
        public async Task<ActionResult<Usuario>> CrearCliet([FromBody] string nombre, string email, string password,string rol)
        {
            Usuario usuario = new Usuario
            {
                Nombre=nombre,
                Email=email,
                Password=password,
                Rol=rol
            };

            _contexto.Usuarios.Add(usuario);
            await _contexto.SaveChangesAsync();
            return Ok(usuario);
        }
        [HttpDelete]
        public async Task<ActionResult<Usuario>> DeleteUser(Guid id)
        {
            var usuario = await _contexto.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            _contexto.Usuarios.Remove(usuario);
            await _contexto.SaveChangesAsync();
            return Ok();
        }



    }
}
