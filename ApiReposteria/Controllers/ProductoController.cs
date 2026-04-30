using ApiReposteria.Data;
using ApiReposteria.Dtos;
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
        public async Task<ActionResult<IEnumerable<AgregarProductoOutput>>> GetProductos()
        {
            var productos =await  _context.Productos.ToArrayAsync();
            var salida =new List<AgregarProductoOutput>();
            foreach(var p in productos)
            {   
                salida.Add(
                    new AgregarProductoOutput
                    {
                        Id = Guid.NewGuid(),
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                NombreCategoria = _context.Categorias.Find(p.CategoriaId).Nombre,
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
            var Categoria = await (from c in _context.Categorias
                                   where c.Nombre == ProductoIn.NombreCategoria
                                   select c).FirstOrDefaultAsync();
            if (Categoria == null)
            {
                return BadRequest("la categoria no existe");
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
    }
}