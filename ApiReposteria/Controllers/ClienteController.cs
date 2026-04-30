using System.Collections.Generic;
using ApiReposteria.Entidades;
using ApiReposteria.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiReposteria.Dtos;

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

            var Clientes = await _contexto.Clientes.ToArrayAsync();
            return Ok(Clientes);

        }
        [HttpPost]
        public async Task<ActionResult<IEnumerable<AgregarClienteDtoOutput>>> CreateCliente([FromBody] AgregarClienteDtoInput ClienteIn)
        {
            
            List<Direccion> direccions = new List<Direccion>();
            Cliente cliente = new Cliente
            {
                Id = Guid.NewGuid(), 
                CI = ClienteIn.CI,
                Extension = ClienteIn.Extension,
                Nombre = ClienteIn.Nombre,
                Telefono = ClienteIn.Telefono
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
                direccions.Add(direccion);
            }
            _contexto.Clientes.Add(cliente);
            await _contexto.SaveChangesAsync();
            return Ok(cliente);


        }

        
    }

}
