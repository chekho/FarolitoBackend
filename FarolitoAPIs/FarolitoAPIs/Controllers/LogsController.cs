using FarolitoAPIs.Data;
using FarolitoAPIs.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FarolitoAPIs.Controllers
{
    public class LogsController : Controller
    {
        private readonly FarolitoDbContext _baseDatos;
        public LogsController(FarolitoDbContext baseDatos)
        {
            _baseDatos = baseDatos;
        }
        /* 
         * Recetas, proveedores, componentes
         * fecha, hora, módulo, usuario, cambio
         */

        [Authorize(Roles = "Administrador")]
        [HttpGet("logs")]
        public async Task<IActionResult> GetLogs()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Usuario no autorizado"
                });
            }

            return Ok();
        }

        [HttpGet("get-modules")]
        public async Task<IActionResult> Getall()
        {
            return Ok(_baseDatos.Modulos.ToList());
        }



    }
}
