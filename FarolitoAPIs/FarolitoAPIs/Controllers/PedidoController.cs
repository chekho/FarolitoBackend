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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
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

            return Ok(pedidoDTOs);
        }


        [HttpGet("obtener-todos-pedidos")]
        public async Task<IActionResult> ObtenerTodosPedidos()
        {
            var pedidos = await _baseDatos.Pedidos
                .Include(p => p.Ventum)
                .ThenInclude(v => v.Detalleventa)
                .ThenInclude(dv => dv.Inventariolampara)
                .ThenInclude(il => il.Receta)
                .Include(p => p.Ventum.Usuario)
                .ToListAsync();

            if (pedidos == null || !pedidos.Any())
            {
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

            return Ok(pedidoDTOs);
        }

        [HttpPut("actualizar-estado")]
        public async Task<IActionResult> ActualizarEstadoPedido([FromBody] ActualizarEstadoPedidoDTO actualizarEstadoPedidoDto)
        {
            if (actualizarEstadoPedidoDto == null || actualizarEstadoPedidoDto.PedidoId <= 0)
            {
                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "ID de pedido no válido"
                });
            }

            var pedido = await _baseDatos.Pedidos.FindAsync(actualizarEstadoPedidoDto.PedidoId);

            if (pedido == null)
            {
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
                    return BadRequest(new AuthResponseDTO
                    {
                        IsSuccess = false,
                        Message = "El estado actual del pedido no permite actualizaciones"
                    });
            }

            _baseDatos.Entry(pedido).State = EntityState.Modified;
            await _baseDatos.SaveChangesAsync();

            return Ok(new AuthResponseDTO
            {
                IsSuccess = true,
                Message = "Estado del pedido actualizado correctamente"
            });
        }

    }
}
