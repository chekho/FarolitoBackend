using FarolitoAPIs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FarolitoAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly FarolitoDbContext _baseDatos;
        public UsuarioController(FarolitoDbContext baseDatos)
        {
            _baseDatos = baseDatos;
        }
        // [HttpGet]
        // [Route("NombresYIdsUsuarios")]
        // public async Task<IActionResult> NombresYIds()
        // {
        //     var nombresYIdsUsuarios = await _baseDatos.Usuarios
        //         .Select(u => new { u.Id, u.Nombre }) // Selecciona el ID y el nombre
        //         .ToListAsync();

        //     return Ok(nombresYIdsUsuarios);
        // }

        [HttpGet]
        [Route("login")]
        public async Task<IActionResult> DetallesUsuario(string Nombre, string Contraseña)
        {
            var usuarioEncontrado = await _baseDatos.Usuarios
                .Join(
                    _baseDatos.DetallesUsuarios,
                    usuario => usuario.DetallesUsuarioId,
                    detalle => detalle.Id,
                    (usuario, detalle) => new
                    {
                        usuario.Id,
                        usuario.Nombre,
                        usuario.Contraseña,
                        usuario.Rol,
                        usuario.Estatus,
                        detalleUsuario = new
                        {
                            detalle.Nombres,
                            detalle.ApellidoM,
                            detalle.ApellidoP,
                            detalle.Correo
                        }
                    }
                )
                .FirstOrDefaultAsync(u => u.Nombre == Nombre && u.Contraseña == Contraseña);

            if (usuarioEncontrado == null)
            {
                var respuestaJson = new
                {
                    error = "Usuario no encontrado"
                };

                return NotFound(respuestaJson);
            }
            return Ok(usuarioEncontrado);
        }

        // POST: api/Usuario/CrearUsuario
        [HttpPost]
        [Route("signin")]
        public async Task<IActionResult> Agregar([FromBody] Usuario usuario)
        {
            await _baseDatos.Usuarios.AddAsync(usuario);
            await _baseDatos.SaveChangesAsync();
            return Ok(usuario);
        }



    }
}
