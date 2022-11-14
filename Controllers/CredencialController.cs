using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using PracticaViamatica.Data;
using PracticaViamatica.Helpers;
using PracticaViamatica.Model;

namespace PracticaViamatica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CredencialController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _accessor;
        public CredencialController(DataContext context, IConfiguration configuration, IHttpContextAccessor accessor)
        {
            _context = context;
            _configuration = configuration;
            _accessor = accessor;
            // this._context.persona.ToList();
            this._context.persona.ToList();
        }

        // GET: api/Credencial
        [HttpPost("Login")]
        public async Task<ActionResult> LoginCliente(DataUserConnect dataUser)
        {
            var uset_temp = await _context.credenciales.FirstOrDefaultAsync(x=> x.usuario.ToLower().Equals(dataUser.usuario));
            if (uset_temp == null)
            {
                return BadRequest("Usuario no encontrado");
            }
            else
            {
                if (uset_temp.password.Equals(dataUser.password))
                {
                    return Ok(JsonConvert.SerializeObject(CearToken(uset_temp)));
                }
                else{
                    return BadRequest("Contraseña incorrecta");
                }
            }
        }

        private string CearToken(Credenciales user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.idCredenciales.ToString()), new Claim(ClaimTypes.Name, user.usuario)
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = System.DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
         [Authorize]
        // GET: api/Personas/5
        [HttpGet("/BuscarUser")]
        public async Task<ActionResult<Persona>> BuscarUsuario()
        {
            try
            {
                int id = TokenVMT.JwtToPayloadUserData(_accessor.HttpContext);
                if (id != 0)
                {
                    var credenciales = await _context.credenciales.FindAsync(id);
                    int idPersona = credenciales.idPerson.Idpersona;
                    var persona = await _context.persona.FindAsync(idPersona);
                    return Ok(persona);
                }
                else
                {
                    return BadRequest("Token invalido");
                }
                
            }
            catch(Exception ex) 
            {
                return BadRequest(ex.Message+"lll");
            }

        }
        private bool CredencialExists(int id)
        {
            return _context.credenciales.Any(e => e.idCredenciales == id);
        }
    }
}
