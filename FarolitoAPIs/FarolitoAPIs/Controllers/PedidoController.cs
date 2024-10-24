using FarolitoAPIs.Data;
using FarolitoAPIs.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Serilog;

namespace FarolitoAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly FarolitoDbContext _baseDatos;
        public PedidoController(FarolitoDbContext baseDatos)
        {
            _baseDatos = baseDatos;
        }

        [Authorize]
        [HttpGet("obtener-pedidos")]
        public async Task<IActionResult> ObtenerPedidosUsuario()
        {
            //Log.Information("Request received to get orders for user.");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                Log.Warning("Unauthorized access attempt: User not authenticated.");

                return Unauthorized(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Usuario no autorizado"
                });
            }

            var pedidos = await _baseDatos.Pedidos
                .Where(p => p.Ventum.UsuarioId == userId)
                .Include(p => p.Ventum)
                .ThenInclude(v => v.Detalleventa)
                .ThenInclude(dv => dv.Inventariolampara)
                .ThenInclude(il => il.Receta)
                .ToListAsync();

            if (pedidos == null || !pedidos.Any())
            {
                //Log.Information("No orders found for user ID {UserId}.", userId);

                return NotFound(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "No se encontraron pedidos"
                });
            }

            var pedidoDTOs = pedidos.Select(p => new PedidoDTO
            {
                Id = p.Id,
                FechaPedido = p.FechaPedido,
                FechaEnvio = p.FechaEnvio,
                FechaEntrega = p.FechaEntrega,
                FechaEntregaAprox = p.FechaEnvio.HasValue ? p.FechaEnvio.Value.AddDays(28) : (DateOnly?)null,
                Estatus = p.Estatus,
                Productos = p.Ventum.Detalleventa.Select(dv => new ProductoDTO
                {
                    RecetaId = dv.Inventariolampara.Receta.Id,
                    InventarioId = dv.Inventariolampara.Id, 
                    Cantidad = dv.Cantidad ?? 0,
                    PrecioUnitario = dv.PrecioUnitario ?? 0,
                    NombreProducto = dv.Inventariolampara.Receta.Nombrelampara
                }).ToList()
            }).ToList();

            //Log.Information("Successfully retrieved {Count} orders for user ID {UserId}.", pedidoDTOs.Count, userId);

            return Ok(pedidoDTOs);
        }


        [HttpGet("obtener-todos-pedidos")]
        public async Task<IActionResult> ObtenerTodosPedidos()
        {
            //Log.Information("Request received to get all orders.");

            var pedidos = await _baseDatos.Pedidos
                .Include(p => p.Ventum)
                .ThenInclude(v => v.Detalleventa)
                .ThenInclude(dv => dv.Inventariolampara)
                .ThenInclude(il => il.Receta)
                .Include(p => p.Ventum.Usuario)
                .ToListAsync();

            if (pedidos == null || !pedidos.Any())
            {
                //Log.Information("No orders found.");

                return NotFound(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "No se encontraron pedidos"
                });
            }

            var pedidoDTOs = pedidos.Select(p => new Pedido2DTO
            {
                Id = p.Id,
                FechaPedido = p.FechaPedido,
                FechaEnvio = p.FechaEnvio,
                FechaEntrega = p.FechaEntrega,
                FechaEntregaAprox = p.FechaEnvio.HasValue ? p.FechaEnvio.Value.AddDays(28) : (DateOnly?)null,
                Estatus = p.Estatus,
                UsuarioId = p.Ventum.UsuarioId,
                NombreUsuario = p.Ventum.Usuario.FullName,
                Productos = p.Ventum.Detalleventa.Select(dv => new ProductoDTO
                {
                    RecetaId = dv.Inventariolampara.Receta.Id,
                    InventarioId = dv.Inventariolampara.Id,
                    Cantidad = dv.Cantidad ?? 0,
                    PrecioUnitario = dv.PrecioUnitario ?? 0,
                    NombreProducto = dv.Inventariolampara.Receta.Nombrelampara
                }).ToList()
            }).ToList();

            //Log.Information("Successfully retrieved {Count} orders.", pedidoDTOs.Count);

            return Ok(pedidoDTOs);
        }

        [HttpPut("actualizar-estado")]
        public async Task<IActionResult> ActualizarEstadoPedido([FromBody] ActualizarEstadoPedidoDTO actualizarEstadoPedidoDto)
        {
            //Log.Information("Request received to update order status for PedidoId: {PedidoId}", actualizarEstadoPedidoDto?.PedidoId);

            if (actualizarEstadoPedidoDto == null || actualizarEstadoPedidoDto.PedidoId <= 0)
            {
                Log.Warning("Invalid order ID provided: {PedidoId}", actualizarEstadoPedidoDto?.PedidoId);

                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "ID de pedido no válido"
                });
            }

            var pedido = await _baseDatos.Pedidos.FindAsync(actualizarEstadoPedidoDto.PedidoId);

            if (pedido == null)
            {
                Log.Warning("Order not found: {PedidoId}", actualizarEstadoPedidoDto.PedidoId);

                return NotFound(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Pedido no encontrado"
                });
            }

            switch (pedido.Estatus)
            {
                case "En Proceso":
                    pedido.Estatus = "Enviado";
                    pedido.FechaEnvio = DateOnly.FromDateTime(DateTime.UtcNow.AddHours(-6));
                    break;
                case "Enviado":
                    pedido.Estatus = "En Camino";
                    break;
                case "En Camino":
                    pedido.Estatus = "Finalizado";
                    pedido.FechaEntrega = DateOnly.FromDateTime(DateTime.UtcNow.AddHours(-6));
                    break;
                default:
                    Log.Warning("Cannot update status for order {PedidoId} with current status: {Estatus}", pedido.Id, pedido.Estatus);
                    return BadRequest(new AuthResponseDTO
                    {
                        IsSuccess = false,
                        Message = "El estado actual del pedido no permite actualizaciones"
                    });
            }

            _baseDatos.Entry(pedido).State = EntityState.Modified;
            await _baseDatos.SaveChangesAsync();

            //Log.Information("Order status updated successfully for PedidoId: {PedidoId}, New Status: {Estatus}", pedido.Id, pedido.Estatus);
            return Ok(new AuthResponseDTO
            {
                IsSuccess = true,
                Message = "Estado del pedido actualizado correctamente"
            });
        }

    }
}
