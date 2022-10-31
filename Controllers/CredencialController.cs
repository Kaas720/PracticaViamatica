using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticaViamatica.Data;
using PracticaViamatica.Model;

namespace PracticaViamatica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CredencialController : ControllerBase
    {
        private readonly DataContext _context;

        public CredencialController(DataContext context)
        {
            _context = context;
            this._context.persona.ToList();
        }

        // GET: api/Credencial
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Credenciales>>> GetCredencial()
        {
            return await _context.credenciales.ToListAsync();
        }

        // GET: api/Credencial/5
        [HttpGet("{id}")]
        public async Task<Credenciales> GetCredencial(int id)
        {
            var credencial = await _context.credenciales.FindAsync(id);

           

            return credencial;
        }

        // PUT: api/Credencial/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCredencial(int id, Credenciales credencial)
        {
            if (id != credencial.idCredenciales)
            {
                return BadRequest();
            }

            _context.Entry(credencial).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CredencialExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Credencial
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Credenciales>> PostCredencial(Credenciales credencial)
        {
            _context.credenciales.Add(credencial);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCredencial", new { id = credencial.idCredenciales }, credencial);
        }

        // DELETE: api/Credencial/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCredencial(int id)
        {
            var credencial = await _context.credenciales.FindAsync(id);
            if (credencial == null)
            {
                return NotFound();
            }

            _context.credenciales.Remove(credencial);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CredencialExists(int id)
        {
            return _context.credenciales.Any(e => e.idCredenciales == id);
        }
    }
}
