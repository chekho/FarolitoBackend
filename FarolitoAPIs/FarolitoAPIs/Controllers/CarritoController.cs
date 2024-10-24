using FarolitoAPIs.Data;
using FarolitoAPIs.DTOs;
using FarolitoAPIs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
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

            //Log.Information("Request received to get user cart");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                Log.Warning("Unauthorized access attempt detected. No user ID found in token");
                return Unauthorized(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Usuario no autorizado"
                });
            }

            //Log.Information("User ID {UserId} found. Retrieving cart items", userId);

            var carrito = await _baseDatos.Carritos
                .Include(c => c.Receta)
                .Where(c => c.UsuarioId == userId)
                .ToListAsync();

            if (carrito == null || !carrito.Any())
            {
                //Log.Information("No items found in the cart for user ID {UserId}.", userId);
                return NotFound(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "No se encontraron elementos en el carrito"
                });
            }

            //Log.Information("Found {CarritoCount} items in the cart for user ID {UserId}.", carrito.Count, userId);

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


            //Log.Information("Inventory retrieved for {RecetaCount} recipes", recetaIds.Count);

            var carritoConEstatus = carrito.Select(c => new
            {
                LamparaId = c.RecetaId,
                UrlImage = c.Receta.Imagen,
                LamparaNombre = c.Receta.Nombrelampara,
                Cantidad = c.Cantidad,
                Estatus = inventario.FirstOrDefault(i => i.RecetaId == c.RecetaId)?.TotalCantidad > 0 ? "disponible" : "agotado"
            }).ToList();

            //Log.Information("Cart data prepared with status for user ID {UserId}.", userId);
            return Ok(carritoConEstatus);
        }

        [Authorize]
        [HttpPost("agregar-carrito")]
        public async Task<IActionResult> AgregarAlCarrito([FromBody] List<CarritoRequestDTO> request)
        {

            //Log.Information("Request received to add items to the cart.");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                //Log.Information("Unauthorized access attempt. No user ID found in the token");
                return Unauthorized(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Usuario no autorizado"
                });
            }

            //Log.Information("User ID {UserId} found. Processing request to add items to cart.", userId);


            if (request == null || !request.Any())
            {

                //Log.Information("Emoty cart addition request from user ID {UserId}.", userId);

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

            //Log.Information("Inventory retrieved for {RecetaCount} recipes.", recetaIds.Count);

            foreach (var item in request)
            {
                var inventarioReceta = inventario.FirstOrDefault(i => i.RecetaId == item.RecetaId);

                if (inventarioReceta == null || inventarioReceta.TotalCantidad < item.Cantidad)
                {

                    Log.Warning("Insufficient stock for recipe ID {RecetaId}. Requested: {Requested}, Available: {Available}.",
                        item.RecetaId, item.Cantidad, inventarioReceta?.TotalCantidad ?? 0);
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
                    //Log.Information("Updated existing cart item for recipe ID {RecetaId} with quantity {Quantity}.",
                        //item.RecetaId, carritoExistente.Cantidad);
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
                    //Log.Information("Added new cart item for recipe ID {RecetaId} with quantity {Quantity}.",
                    //    item.RecetaId, item.Cantidad);

                }
            }

            await _baseDatos.SaveChangesAsync();


            //Log.Information("Cart updated successfully for user ID {UserId}.", userId);


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

            //Log.Information("Request received to delete items from the cart");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                Log.Warning("Unauthorized access attempt. No user ID found in the token.");
                return Unauthorized(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Usuario no autorizado"
                });
            }

            //Log.Information("User ID {UserId} found. Processing deletion request.", userId);

            if (request == null || !request.Any())
            {
                Log.Warning("Empty deletion request from user ID {UserId}. No item IDs provided.", userId);
                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "No se proporcionaron IDs de elementos del carrito"
                });
            }

            var lamparaIdsAEliminar = request.Select(r => r.Id).ToList();
            //Log.Information("Attempting to delete {Count} item(s) from the cart for user ID {UserId}.", lamparaIdsAEliminar.Count, userId);

            var elementosCarrito = await _baseDatos.Carritos
                .Where(c => c.UsuarioId == userId && lamparaIdsAEliminar.Contains(c.RecetaId))
                .ToListAsync();

            if (elementosCarrito == null || !elementosCarrito.Any())
            {
                Log.Warning("No items found in the cart for user ID {UserId} matching the provided IDs.", userId);
                return NotFound(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "No se encontraron elementos del carrito para eliminar"
                });
            }

            //Log.Information("Found {Count} items(s) to delete for user ID {UserId}.", elementosCarrito.Count, userId);

            _baseDatos.Carritos.RemoveRange(elementosCarrito);
            await _baseDatos.SaveChangesAsync();

            //Log.Information("Successfuly deleted {Count} item(s) from the cart for user ID {UserId}.", elementosCarrito.Count, userId);

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

            //Log.Information("Request received to update cart quantities.");

            if (request == null || !request.Any())
            {

                Log.Warning("Empty update request. No items provided to update");
                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "No se proporcionaron elementos para actualizar"
                });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {

                Log.Warning("Unauthorized access attempt. No user ID found in the token");
                return Unauthorized(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Usuario no autorizado"
                });
            }

            //Log.Information("User ID {UserId} found. Processing update request", userId);

            foreach (var item in request)
            {
                //Log.Information("Processing update for RecetaId: {RecetaId} with new quantity: {NuevaCantidad}");

                var carritoItem = await _baseDatos.Carritos
                    .FirstOrDefaultAsync(c => c.UsuarioId == userId && c.RecetaId == item.RecetaId);

                if (carritoItem == null)
                {
                    Log.Warning("No cart item found for user ID {UserId} and RecetaId {RecetaId}", userId, item.RecetaId);
                    return NotFound(new AuthResponseDTO
                    {
                        IsSuccess = false,
                        Message = $"No se encontró el elemento del carrito con RecetaId: {item.RecetaId}"
                    });
                }

                var totalExistencias = await _baseDatos.Inventariolamparas
                    .Where(il => il.RecetaId == item.RecetaId)
                    .SumAsync(il => il.Cantidad ?? 0);

                
                //Log.Information("Total stock available for RecetaId: {RecetaId} is {TotalExistencias}", item.RecetaId, totalExistencias);
                
                if (item.NuevaCantidad > totalExistencias)
                {
                    Log.Warning("Insufficient stock for RecetaId {RecetaId}, Requested: {NuevaCantidad}, Available: {TotalExistencias}", item.RecetaId, item.NuevaCantidad, totalExistencias);
                    return BadRequest(new AuthResponseDTO
                    {
                        IsSuccess = false,
                        Message = $"No hay suficientes existencias en inventario para la receta con RecetaId: {item.RecetaId}"
                    });
                }

                carritoItem.Cantidad = item.NuevaCantidad;
                //Log.Information("Updated cart item RecetaId {RecetaId} to new quantity: {NuevaCantidad}", item.RecetaId, item.NuevaCantidad);
            }

            await _baseDatos.SaveChangesAsync();

            //Log.Information("Cart quantities updated successfuly for user ID {UserId}",userId);

            return Ok(new AuthResponseDTO
            {
                IsSuccess = true,
                Message = "Cantidad del carrito actualizada correctamente"
            });
        }
    }
}
