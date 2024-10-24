using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace FarolitoAPIs.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestErrorLogController : ControllerBase
    {
        [HttpGet("error")]
        public IActionResult GetError()
        {
            try
            {
                throw new Exception("Esta es una prueba de excepcion de Serilog");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error, este test ha sido exitoso");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }

        }

    }
}
