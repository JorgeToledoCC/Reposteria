using ApiReposteria.Data;
using ApiReposteria.Dtos;
using ApiReposteria.Dtos.Producto;
using ApiReposteria.Entidades;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace ApiReposteria.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly ReposteriaContext _context;
        public ProductosController(ReposteriaContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetProductoOutput>>> GetProductos()
        {
            var productos = await _context.Productos.ToArrayAsync();
            var salida = new List<GetProductoOutput>();
            var name = "sin definir";
            foreach (var p in productos)
            {
                var categorinombre = _context.Categorias.Find(p.CategoriaId);
                if (categorinombre != null)
                {
                    name = categorinombre.Nombre;
                }
                salida.Add(
                    new GetProductoOutput
                    {
                        Id = Guid.NewGuid(),
                        Nombre = p.Nombre,
                        Descripcion = p.Descripcion,
                        NombreCategoria = name,
                        Precio = p.Precio,
                        stock = p.Stock
                    }
                );

            }
            return Ok(salida);
        }

        [HttpPost]
        public async Task<ActionResult<AgregarProductoOutput>> CreateProducto([FromBody] AgregarProductoInput ProductoIn)
        {
            var existe = await _context.Productos
            .AnyAsync(p => p.Nombre.ToLower() == ProductoIn.Nombre.ToLower());

            if (existe)
            {
            return BadRequest($"El producto '{ProductoIn.Nombre}' ya existe en el catálogo.");
            }

            var Categoria = await (from c in _context.Categorias
                        where c.Nombre == ProductoIn.NombreCategoria
                        select c).FirstOrDefaultAsync();

            if (Categoria == null)
            {
                return BadRequest("La categoría no existe.");
            }

            Producto producto = new Producto
            {
                Id = Guid.NewGuid(),
                Nombre = ProductoIn.Nombre,
                Descripcion = ProductoIn.Descripcion,
                Categoria = Categoria,
                Precio = ProductoIn.Precio,
                ImgUrl = ProductoIn.ImgUrl,
                Stock = ProductoIn.stock
            };

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            var resultado = new AgregarProductoOutput
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Precio = producto.Precio,
                NombreCategoria = Categoria.Nombre,
                stock = producto.Stock
            };

            return CreatedAtAction(nameof(CreateProducto), new { id = producto.Id }, resultado);
        }

        [HttpGet("cat")]
        public async Task<ActionResult<IEnumerable<GetProductoOutput>>> GetProductocat([FromQuery] string categorianombre)
        {
            var cat = (from c in _context.Categorias
                        where c.Nombre == categorianombre
                        select c).FirstOrDefault();
            if(cat == null)
            {
                return BadRequest("la categoria no existe");
            }
            var productos = await _context.Productos.ToArrayAsync();
            var salida = new List<GetProductoOutput>();
            var name = "sin definir";
            foreach (var p in productos)
            {
                var categorinombre = _context.Categorias.Find(p.CategoriaId);
                if (categorinombre != null)
                {
                    name = categorinombre.Nombre;
                }
                if(categorianombre == name)
                {
                    
                salida.Add(
                    new GetProductoOutput
                    {
                        Id = Guid.NewGuid(),
                        Nombre = p.Nombre,
                        Descripcion = p.Descripcion,
                        NombreCategoria = name,
                        Precio = p.Precio,
                        stock = p.Stock
                    }
                );
                }

            }
            return Ok(salida);
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchProducto(Guid id, [FromBody] AgregarProductoInput input)
        {
            var producto = await _context.Productos.Include(p => p.Categoria).FirstOrDefaultAsync(p => p.Id == id);
            if (producto == null) return NotFound();

            if (!string.IsNullOrEmpty(input.Nombre)) producto.Nombre = input.Nombre;
            if (!string.IsNullOrEmpty(input.Descripcion)) producto.Descripcion = input.Descripcion;
            if (input.Precio > 0) producto.Precio = input.Precio;
            if (input.stock >= 0) producto.Stock = input.stock;
            if (!string.IsNullOrEmpty(input.ImgUrl)) producto.ImgUrl = input.ImgUrl;

            if (!string.IsNullOrEmpty(input.NombreCategoria) && producto.Categoria.Nombre != input.NombreCategoria)
            {
                var nuevaCat = await _context.Categorias.FirstOrDefaultAsync(c => c.Nombre == input.NombreCategoria);
                if (nuevaCat != null) producto.Categoria = nuevaCat;
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}