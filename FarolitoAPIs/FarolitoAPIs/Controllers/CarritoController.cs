using FarolitoAPIs.Data;
using FarolitoAPIs.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FarolitoAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarritoController : ControllerBase
    {
        private readonly FarolitoDbContext _baseDatos;
        public CarritoController(FarolitoDbContext baseDatos)
        {
            _baseDatos = baseDatos;
        }

        [HttpGet("usuario-carrito")]
        [Authorize] // Asegúrate de que este endpoint requiere autorización
        public async Task<IActionResult> ObtenerCarritoUsuario()
        {
            // Obtener el ID del usuario logueado
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Usuario no autorizado"
                });
            }

            // Obtener el carrito del usuario logueado
            var carrito = await _baseDatos.Carritos
                .Include(c => c.Receta)
                .Where(c => c.UsuarioId == userId)
                .Select(c => new
                {
                    RecetaNombre = c.Receta.Nombrelampara,
                    Cantidad = c.Cantidad
                })
                .ToListAsync();

            if (carrito == null || !carrito.Any())
            {
                return NotFound(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "No se encontraron elementos en el carrito"
                });
            }

            return Ok(carrito);
        }
    }
}
