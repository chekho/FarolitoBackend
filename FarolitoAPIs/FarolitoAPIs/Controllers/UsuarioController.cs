using FarolitoAPIs.Models;
using Microsoft.AspNetCore.Http;
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

      

    }
}
