using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using DurableTask.Core.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PracticaViamatica.Data;
using PracticaViamatica.Helpers;
using PracticaViamatica.Model;

namespace PracticaViamatica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductoesController : ControllerBase
    {
        private readonly DataContext _context;


        public ProductoesController(DataContext context, IHttpContextAccessor accessor)
        {
            _context = context;
        }

        // GET: api/Productoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> Getproducto()
        {
            return await _context.producto.ToListAsync();
        }

        // GET: api/Productoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {

            var producto = await _context.producto.FindAsync(id);

            if (producto == null)
            {
                return NotFound();
            }

            return producto;
        }

        // PUT: api/Productoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutProductoGeneral(Producto poducto)
        {
            try
            {
                _context.Entry(poducto).State = EntityState.Modified;
               
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateConcurrencyException msj)
            {
                return Content("Fallo la actualizacion: " + msj);
            }
        }
        [HttpPut("/editar")]
        public async Task<IActionResult> PutProducto(List<productoCompra> productos)
        {
            ///List<Producto> producto = JsonConvert.DeserializeObject<List<Producto>>((string)productos) ;
            try
            {
                
                foreach (productoCompra product in productos)
                {
                    Producto editProduct = await _context.producto.FindAsync(product.idProducto);
                    if (editProduct.cantidadDisponible < product.cantidadComprada) {
                        return NotFound("Uno o mas productos agotados");
                    }
                    editProduct.cantidadDisponible = editProduct.cantidadDisponible - product.cantidadComprada;
                    _context.Entry(editProduct).State = EntityState.Modified;

                }
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException msj)
            {

                return Content("Fallo la actualizacion: "+msj);
            }

            return NoContent();
        }

        // POST: api/Productoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Producto>> PostProducto(Producto producto)
        {
            _context.producto.Add(producto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProducto", new { id = producto.idProducto }, producto);
        }

        // DELETE: api/Productoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            var producto = await _context.producto.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            _context.producto.Remove(producto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductoExists(int id)
        {
            return _context.producto.Any(e => e.idProducto == id);
        }
    }
}
