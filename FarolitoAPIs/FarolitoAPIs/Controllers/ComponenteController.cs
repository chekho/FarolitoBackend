using FarolitoAPIs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FarolitoAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComponenteController : ControllerBase
    {
        private readonly FarolitoDbContext _baseDatos;
        public ComponenteController(FarolitoDbContext baseDatos)
        {
            _baseDatos = baseDatos;
        }

        [HttpGet]
        [Route("componentes")]
        public async Task<IActionResult> lista()
        {
            var listaComponentes = await _baseDatos.Componentes.ToListAsync();

            return Ok(listaComponentes);
        }
    }
}
