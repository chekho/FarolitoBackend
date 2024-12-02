using FarolitoAPIs.Data;
using FarolitoAPIs.DTOs;
using FarolitoAPIs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using FarolitoAPIs.Services;
using FarolitoAPIs.Migrations;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FarolitoAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        private readonly FarolitoDbContext _baseDatos;
        private readonly IPedidoService _pedidoService;

        public VentaController(FarolitoDbContext baseDatos, IPedidoService pedidoService)
        {
            _baseDatos = baseDatos;
            _pedidoService = pedidoService;
        }

        [Authorize]
        [HttpPost("venta-lampara")]
        public async Task<IActionResult> VerificarInventarioLamparas([FromBody] List<ComponentesRequestDTO> request)
        {
            //Log.Information("Received request to verify inventory for lamps.");

            if (request == null || !request.Any())
            {
                Log.Warning("No product IDs provided in the request.");

                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "No se proporcionaron IDs de producto"
                });
            }

            var recetaIds = request.Select(r => r.Id).ToList();
            var cantidadesSolicitadas = request.ToDictionary(r => r.Id, r => r.Cantidad);

            var recetas = await _baseDatos.Receta
                .Include(r => r.Inventariolamparas)
                .Where(r => recetaIds.Contains(r.Id))
                .ToListAsync();

            if (recetas == null || !recetas.Any())
            {
                Log.Warning("No products found for the provided IDs: {RecetaIds}", string.Join(", ", recetaIds));

                return NotFound(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "No se encontraron productos"
                });
            }

            var recetasExistenciaDTO = recetas.Select(r => new RecetaExistenciaDTO
            {
                RecetaId = r.Id,
                Nombre = r.Nombrelampara,
                TotalCantidad = r.Inventariolamparas.Sum(il => il.Cantidad ?? 0),
                Existe = r.Inventariolamparas.Sum(il => il.Cantidad ?? 0) >= cantidadesSolicitadas[r.Id]
            }).ToList();

            bool allExist = recetasExistenciaDTO.All(r => r.Existe);

            if (!allExist)
            {
                Log.Warning("Some products do not have sufficient inventory.");

                return Ok(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Algunos productos no tienen suficiente cantidad en inventario"
                });
            }

            var usuarioId = User?.FindFirstValue(ClaimTypes.NameIdentifier);

            var nuevaVenta = new Ventum
            {
                Fecha = DateTime.UtcNow.AddHours(-6),
                UsuarioId = usuarioId,
                Descuento = null,
                Folio = LotesVentas.GenerarLote(DateTime.UtcNow.AddHours(-6))
            };

            _baseDatos.Venta.Add(nuevaVenta);
            await _baseDatos.SaveChangesAsync();

            //Log.Information("New sale created with ID: {VentaId}", nuevaVenta.Id);

            foreach (var receta in recetas)
            {
                int cantidadRestante = cantidadesSolicitadas[receta.Id];
                var inventarioOrdenado = receta.Inventariolamparas.OrderBy(il => il.FechaCreacion).ToList();
                double precioUnitarioPromedio = receta.Inventariolamparas
                    .Where(il => il.Precio.HasValue)
                    .Average(il => il.Precio.Value) * 1.2;

                foreach (var item in inventarioOrdenado)
                {
                    if (cantidadRestante <= 0)
                        break;

                    if (item.Cantidad.HasValue && item.Cantidad.Value > 0)
                    {
                        int cantidadADescontar = Math.Min(cantidadRestante, item.Cantidad.Value);
                        item.Cantidad -= cantidadADescontar;
                        cantidadRestante -= cantidadADescontar;

                        double precioUnitario = item.Precio.HasValue ? item.Precio.Value * 1.2 : 0;

                        var nuevoDetalleVenta = new Detalleventum
                        {
                            Cantidad = cantidadADescontar,
                            PrecioUnitario = precioUnitarioPromedio,
                            VentaId = nuevaVenta.Id,
                            InventariolamparaId = item.Id
                        };

                        _baseDatos.Detalleventa.Add(nuevoDetalleVenta);
                        Log.Information(
                            "Added sale detail for lamp ID: {LampId}, Quantity: {Cantidad}, Unit Price: {PrecioUnitario}",
                            item.Id, cantidadADescontar, precioUnitarioPromedio);
                    }
                }
            }

            var nuevoPedido = new Pedido
            {
                FechaPedido = DateOnly.FromDateTime(DateTime.UtcNow.AddHours(-6)),
                Estatus = "En Proceso",
                VentumId = nuevaVenta.Id
            };

            _baseDatos.Pedidos.Add(nuevoPedido);
            //Log.Information("New order created with ID: {PedidoId}", nuevoPedido.Id);


            var elementosCarrito = await _baseDatos.Carritos
                .Where(c => c.UsuarioId == usuarioId && recetaIds.Contains(c.RecetaId))
                .ToListAsync();

            if (elementosCarrito != null && elementosCarrito.Any())
            {
                _baseDatos.Carritos.RemoveRange(elementosCarrito);
                //Log.Information("Removed items from cart for user ID: {UsuarioId}", usuarioId);
            }

            await _baseDatos.SaveChangesAsync();

            try
            {
                if (usuarioId != null) await _pedidoService.EnviarCorreoConfirmacionPedidoAsync(nuevaVenta.Id, usuarioId);
            }
            catch (Exception ex)
            {
                Log.Error($"Error al enviar el correo de confirmación de compra: {ex.Message}");
            }

            return Ok(new AuthResponseDTO
            {
                IsSuccess = true,
                Message = "Venta registrada correctamente"
            });
        }

        [AllowAnonymous]
        [HttpGet("ventas-usuario")]
        public async Task<IActionResult> GetVEntasUsuario([FromQuery] string clienteId)
        {
            var compras = (from v in _baseDatos.Venta
                           join dv in _baseDatos.Detalleventa on v.Id equals dv.VentaId
                           join p in _baseDatos.Inventariolamparas on dv.InventariolamparaId equals p.Id
                           where v.UsuarioId == clienteId
                           group new { dv, v } by new { v.Id, v.Fecha, v.Folio } into g
                           select new
                           {
                               VentaId = g.Key.Id,
                               Folio = g.Key.Folio,
                               Fecha = g.Key.Fecha,
                               Cantidad = g.Sum(x => x.dv.Cantidad),
                               Total = g.Sum(x => x.dv.Cantidad * x.dv.PrecioUnitario),
                               Metodo = "Tarjeta"
                           }).ToList();
            var cliente = _baseDatos.Users.Where(u => u.Id == clienteId).FirstOrDefault();

            if(cliente == null)
            {
                return BadRequest("Cliente no encontrado");
            }

            return Ok(new {
                Cliente = cliente,
                Compras = compras
            });
        }

        [AllowAnonymous]
        [HttpGet("ventas-usuario-detalle")]
        public async Task<IActionResult> GetVentasUsuarioDetalles([FromQuery] int ventaId, [FromQuery] string clienteId)
        {
            var info = _baseDatos.Detalleventa.Include(dv => dv.Venta).Include(dv => dv.Inventariolampara).ThenInclude(il => il.Receta).Where(dv => dv.VentaId == ventaId && dv.Venta.UsuarioId == clienteId).ToList();

            if (info == null)
            {
                Log.Warning("Unauthorized access attempt. User ID is unauthorized.");

                return Unauthorized(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Error en la información"
                });
            }
            var venta = info[0].Venta;
            var detallesVenta = new
            {
                Venta = new
                {
                    id = venta.Id,
                    fecha = venta.Fecha,
                    descuento = venta.Descuento ?? 0,
                    folio = venta.Folio,
                },
                detallesVenta = from dv in info
                                select new
                                {
                                    Lampara = dv.Inventariolampara.Receta.Nombrelampara,
                                    Imagen = dv.Inventariolampara.Receta.Imagen,
                                    cantidad = dv.Cantidad,
                                    precioUnitario = dv.PrecioUnitario
                                }
            };
            return Ok(detallesVenta);
        }


    }
}