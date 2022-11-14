using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PracticaViamatica.Data;
using PracticaViamatica.Helpers;
using PracticaViamatica.Model;

namespace PracticaViamatica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonasController : ControllerBase
    {
        private readonly DataContext _context;

        public PersonasController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Personas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Persona>>> GetPersona()
        {
            return await _context.persona.ToListAsync();
        }

        // PUT: api/Personas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> PutPersona(Persona persona)
        {
            try
            {
                if (!PersonaExists(persona.Idpersona))
                {
                    return NotFound(new ApiResultDto()
                    {
                        CodigoRetorno = "404",
                        MensajeRetorno = "Usuario no encontrado"
                    });
                }
                object[] parameters = new object[] {
                    persona.nombre,
                    persona.apellido,
                    persona.correo,
                    persona.telefono,
                    persona.direccionDomicilio,
                    persona.direccionTrabajo
                 };
                if (_context.DoExecSP("CreatePersona {0}, {1}, {2}, {3}, {4}, {5}", parameters) != 0)
                    return Ok();
                else
                    return BadRequest("Error al crear persona sp");
            }
            catch (Exception ex)
            {
                return BadRequest("Error al crear persona: " + ex.Message);
            }
        }

        [HttpPut("spEditar")]
        public async Task<IActionResult> PutPersonaConSP(Persona persona)
        {
            try
            {
                if (!PersonaExists(persona.Idpersona))
                {
                    return NotFound(new ApiResultDto()
                    {
                        CodigoRetorno = "404",
                        MensajeRetorno = "Usuario no encontrado"
                    });
                }
                object[] parameters = new object[] {
                    persona.Idpersona,
                    persona.nombre,
                    persona.apellido,
                    persona.correo,
                    persona.telefono,
                    persona.direccionDomicilio,
                    persona.direccionTrabajo,
                     new SqlParameter() { ParameterName = "@mensaje",Direction = System.Data.ParameterDirection.Output, Size = 50 },
                 };
                string mensaje = "";
               if (_context.DoExecSP("UpdatePersona {0}, {1}, {2}, {3}, {4}, {5}, {6} , {7} OUT", parameters, ref mensaje) != 0)
                    return Ok(new ApiResultDto()
                    {
                        CodigoRetorno = "200",
                        MensajeRetorno = mensaje
                    });
                else
                {
                    return BadRequest(new ApiResultDto()
                    {
                        CodigoRetorno = "404",
                        MensajeRetorno = mensaje
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error al actualizar usuario: "+ex.Message);
            }
        }

        // POST: api/Personas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Persona>> PostPersona(Persona persona)
        {
            _context.persona.Add(persona);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPersona", new { id = persona.Idpersona }, persona);
        }

        // DELETE: api/Personas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersona(int id)
        {
            if (!PersonaExists(id))
            {
                return NotFound(new ApiResultDto()
                {
                    CodigoRetorno = "404",
                    MensajeRetorno = "Usuario no encontrado"
                });
            }
            try
            {
                object[] parameters = new object[] {
                    id,
                 };
                if (_context.DoExecSP("DelatePersona {0}", parameters) != 0)
                    return Ok();
                else
                    return BadRequest("Error al eliminar usuario");
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest("Error al eliminar usuario: " + ex.Message);
            }
        }

        private bool PersonaExists(int id)
        {
            return _context.persona.Any(e => e.Idpersona == id);
        }
    }
}
