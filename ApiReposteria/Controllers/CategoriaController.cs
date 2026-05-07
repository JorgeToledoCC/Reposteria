using ApiReposteria.Data;
using ApiReposteria.Dtos.Categoria;
using ApiReposteria.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 

namespace ApiReposteria.Controllers
{
    
[ApiController]
[Route("api/[controller]")]
public class CategoriaController : ControllerBase
{
    private readonly ReposteriaContext _context;
    public CategoriaController(ReposteriaContext context) => _context = context;

    [HttpGet("mostrar")]
    public async Task<ActionResult<IEnumerable<MostrarCategoriaOutput>>> GetCategoriasSimple()
    {
        var categorias = await _context.Categorias.ToListAsync();
        List<MostrarCategoriaOutput> salida = new List<MostrarCategoriaOutput>();
        foreach(var categoria in categorias)
            {
                salida.Add(new MostrarCategoriaOutput
                {
                    Nombre = categoria.Nombre,
                    Descripcion = categoria.Descripcion
                });
            } 

        return Ok(salida);
    }
    [HttpPost]
public async Task<ActionResult<AgregarCategoriaOutput>> CreateCategoria([FromBody] AgregarCategoriaInput categoriaIn)
{
    var existe = await _context.Categorias
        .AnyAsync(c => c.Nombre.ToLower() == categoriaIn.Nombre.ToLower());

    if (existe)
    {
        return BadRequest($"La categoría '{categoriaIn.Nombre}' ya existe.");
    }

    Categoria categoria = new Categoria
    {
        Id = Guid.NewGuid(),
        Nombre = categoriaIn.Nombre,
        Descripcion = categoriaIn.Descripcion
    };

    _context.Categorias.Add(categoria);
    await _context.SaveChangesAsync();

    var salida = new AgregarCategoriaOutput
    {
        Id = categoria.Id,
        Nombre = categoria.Nombre,
        Descripcion = categoria.Descripcion
    };

    return CreatedAtAction(nameof(CreateCategoria), new { id = categoria.Id }, salida);
}
    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchCategoria(Guid id, [FromBody] AgregarCategoriaInput input)
    {
        var categoria = await _context.Categorias.FindAsync(id);
        if (categoria == null) return NotFound();

        if (!string.IsNullOrEmpty(input.Nombre)) categoria.Nombre = input.Nombre;
        if (!string.IsNullOrEmpty(input.Descripcion)) categoria.Descripcion = input.Descripcion;

        await _context.SaveChangesAsync();
        return NoContent();
    }
    
}
}