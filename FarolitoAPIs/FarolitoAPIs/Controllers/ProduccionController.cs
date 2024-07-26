using FarolitoAPIs.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FarolitoAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProduccionController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet("test")]
        public async Task<ActionResult<AuthResponseDTO>> Prueba()
        {
            return Ok("all well ;3");
        }
    }
}
