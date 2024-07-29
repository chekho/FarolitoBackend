using FarolitoAPIs.Data;
using FarolitoAPIs.DTOs;
using FarolitoAPIs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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

        [Authorize]
        [HttpGet("usuario-carrito")] 
        public async Task<IActionResult> ObtenerCarritoUsuario()
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

            var carrito = await _baseDatos.Carritos
                .Include(c => c.Receta)
                .Where(c => c.UsuarioId == userId)
                .ToListAsync();

            if (carrito == null || !carrito.Any())
            {
                return NotFound(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "No se encontraron elementos en el carrito"
                });
            }

            var recetaIds = carrito.Select(c => c.RecetaId).ToList();
            var inventario = await _baseDatos.Inventariolamparas
                .Where(il => recetaIds.Contains(il.RecetaId))
                .GroupBy(il => il.RecetaId)
                .Select(g => new
                {
                    RecetaId = g.Key,
                    TotalCantidad = g.Sum(il => il.Cantidad ?? 0)
                })
                .ToListAsync();

            var carritoConEstatus = carrito.Select(c => new
            {
                LamparaId = c.RecetaId,
                LamparaNombre = c.Receta.Nombrelampara,
                Cantidad = c.Cantidad,
                Estatus = inventario.FirstOrDefault(i => i.RecetaId == c.RecetaId)?.TotalCantidad > 0 ? "disponible" : "agotado"
            }).ToList();

            return Ok(carritoConEstatus);
        }

        [Authorize]
        [HttpPost("agregar-carrito")]
        public async Task<IActionResult> AgregarAlCarrito([FromBody] List<CarritoRequestDTO> request)
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

            if (request == null || !request.Any())
            {
                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "No se proporcionaron elementos para agregar al carrito"
                });
            }

            var recetaIds = request.Select(r => r.RecetaId).ToList();
            var inventario = await _baseDatos.Inventariolamparas
                .Where(il => recetaIds.Contains(il.RecetaId))
                .GroupBy(il => il.RecetaId)
                .Select(g => new
                {
                    RecetaId = g.Key,
                    TotalCantidad = g.Sum(il => il.Cantidad ?? 0)
                })
                .ToListAsync();

            foreach (var item in request)
            {
                var inventarioReceta = inventario.FirstOrDefault(i => i.RecetaId == item.RecetaId);

                if (inventarioReceta == null || inventarioReceta.TotalCantidad < item.Cantidad)
                {
                    return BadRequest(new AuthResponseDTO
                    {
                        IsSuccess = false,
                        Message = $"No hay suficiente cantidad en inventario para la receta ID: {item.RecetaId}"
                    });
                }
            }

            foreach (var item in request)
            {
                var carritoExistente = await _baseDatos.Carritos
                    .FirstOrDefaultAsync(c => c.UsuarioId == userId && c.RecetaId == item.RecetaId);

                if (carritoExistente != null)
                {
                    carritoExistente.Cantidad += item.Cantidad;
                }
                else
                {
                    var nuevoCarrito = new Carrito
                    {
                        UsuarioId = userId,
                        RecetaId = item.RecetaId,
                        Cantidad = item.Cantidad
                    };
                    await _baseDatos.Carritos.AddAsync(nuevoCarrito);
                }
            }

            await _baseDatos.SaveChangesAsync();

            return Ok(new AuthResponseDTO
            {
                IsSuccess = true,
                Message = "Elementos agregados al carrito exitosamente"
            });
        }
        [HttpDelete("eliminar-del-carrito")]
        [Authorize] 
        public async Task<IActionResult> EliminarDelCarrito([FromBody] List<IdRequestCarritoDTO> request)
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

            if (request == null || !request.Any())
            {
                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "No se proporcionaron IDs de elementos del carrito"
                });
            }

            var lamparaIdsAEliminar = request.Select(r => r.Id).ToList();

            var elementosCarrito = await _baseDatos.Carritos
                .Where(c => c.UsuarioId == userId && lamparaIdsAEliminar.Contains(c.RecetaId))
                .ToListAsync();

            if (elementosCarrito == null || !elementosCarrito.Any())
            {
                return NotFound(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "No se encontraron elementos del carrito para eliminar"
                });
            }

            _baseDatos.Carritos.RemoveRange(elementosCarrito);
            await _baseDatos.SaveChangesAsync();

            return Ok(new AuthResponseDTO
            {
                IsSuccess = true,
                Message = "Elementos eliminados del carrito exitosamente"
            });
        }

        [Authorize]
        [HttpPatch("actualizar-carrito")]
        public async Task<IActionResult> ActualizarCantidadCarrito([FromBody] List<ActualizarCarritoRequestDTO> request)
        {
            if (request == null || !request.Any())
            {
                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "No se proporcionaron elementos para actualizar"
                });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Usuario no autorizado"
                });
            }

            foreach (var item in request)
            {
                var carritoItem = await _baseDatos.Carritos
                    .FirstOrDefaultAsync(c => c.UsuarioId == userId && c.RecetaId == item.RecetaId);

                if (carritoItem == null)
                {
                    return NotFound(new AuthResponseDTO
                    {
                        IsSuccess = false,
                        Message = $"No se encontró el elemento del carrito con RecetaId: {item.RecetaId}"
                    });
                }

                var totalExistencias = await _baseDatos.Inventariolamparas
                    .Where(il => il.RecetaId == item.RecetaId)
                    .SumAsync(il => il.Cantidad ?? 0);

                if (item.NuevaCantidad > totalExistencias)
                {
                    return BadRequest(new AuthResponseDTO
                    {
                        IsSuccess = false,
                        Message = $"No hay suficientes existencias en inventario para la receta con RecetaId: {item.RecetaId}"
                    });
                }

                carritoItem.Cantidad = item.NuevaCantidad;
            }

            await _baseDatos.SaveChangesAsync();

            return Ok(new AuthResponseDTO
            {
                IsSuccess = true,
                Message = "Cantidad del carrito actualizada correctamente"
            });
        }
    }
}
