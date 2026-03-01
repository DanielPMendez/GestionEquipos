using GestionEquipos.Models;
using GestionEquipos.ServiceLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionEquipos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _usuarioService;

        public AuthenticationController(IAuthService usuarioService) => _usuarioService = usuarioService;

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login(Login login)
        {
            if (string.IsNullOrEmpty(login.username) || string.IsNullOrEmpty(login.password))
            {
                throw new ArgumentException("El usuario y/o la contraseña son requeridos.");
            }

            var token = await _usuarioService.LoginAsync(login.username, login.password);
            if (token == null)
            {
                return Unauthorized(new { Message = "Credenciales inválidas" });
            }
            return Ok(new { Token = token });
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register(Register register)
        {
            int newUserId = await _usuarioService.RegistrarUsuarioAsync(register.username, register.password, register.idRol);
            if (newUserId <= 0)
            {
                return BadRequest(new { Message = "No se pudo registrar el usuario" });
            }
            return CreatedAtAction(nameof(Login), new { id = newUserId }, null);
        }
    }
}
