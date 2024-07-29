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
    public class VentaController : ControllerBase
    {
        private readonly FarolitoDbContext _baseDatos;
        public VentaController(FarolitoDbContext baseDatos)
        {
            _baseDatos = baseDatos;
        }

        [Authorize]
        [HttpPost("venta-lampara")]
        public async Task<IActionResult> VerificarInventarioLamparas([FromBody] List<ComponentesRequestDTO> request)
        {
            if (request == null || !request.Any())
            {
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
                Folio = null
            };

            _baseDatos.Venta.Add(nuevaVenta);
            await _baseDatos.SaveChangesAsync();

            foreach (var receta in recetas)
            {
                int cantidadRestante = cantidadesSolicitadas[receta.Id];
                var inventarioOrdenado = receta.Inventariolamparas.OrderBy(il => il.FechaCreacion).ToList();

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
                            PrecioUnitario = precioUnitario,
                            VentaId = nuevaVenta.Id,
                            InventariolamparaId = item.Id
                        };

                        _baseDatos.Detalleventa.Add(nuevoDetalleVenta);
                    }
                }
            }

            var elementosCarrito = await _baseDatos.Carritos
                .Where(c => c.UsuarioId == usuarioId && recetaIds.Contains(c.RecetaId))
                .ToListAsync();

            if (elementosCarrito != null && elementosCarrito.Any())
            {
                _baseDatos.Carritos.RemoveRange(elementosCarrito);
            }

            await _baseDatos.SaveChangesAsync();

            return Ok(new AuthResponseDTO
            {
                IsSuccess = true,
                Message = "Venta registrada correctamente"
            });
        }

    }
}
