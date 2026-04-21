using System.Collections;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pasteleria.DTO.Usuario.AgregarUsuario;
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
        public async Task<ActionResult<AgregarUsuarioOutput>> CrearCliet([FromBody] AgregarUsuarioinput usuario)
        {
            var entrada = new Usuario
            {
                Id = Guid.NewGuid(),
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Password = usuario.Password,
                Rol = "Indefinido"
            };
            entrada.Rol = "Cliente";
            _contexto.Usuarios.Add(entrada);
            await _contexto.SaveChangesAsync();
            var salida = new AgregarUsuarioOutput
            {
                Id = entrada.Id,
                Nombre = entrada.Nombre,
                Email = entrada.Email,
                Password = entrada.Password,
                Rol  = entrada.Rol
            };
            return CreatedAtAction( nameof(Getuserid),new { id = entrada.Id },salida); //lo busca por el id, verifica si esta creado y lo devuelve, y si no te jodes wei equisdedxddxdxd
            
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
