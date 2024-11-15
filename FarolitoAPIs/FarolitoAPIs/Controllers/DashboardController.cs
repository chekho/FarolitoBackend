using FarolitoAPIs.Data;
using FarolitoAPIs.DTOs;
using FarolitoAPIs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace FarolitoAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly FarolitoDbContext _context;
        public DashboardController(FarolitoDbContext context)
        {
            _context = context;
        }
        [HttpGet("VentasProductos")]
        public async Task<ActionResult> GetVentasProductos(DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            var query = _context.Detalleventa.AsQueryable();

            if (fechaInicio.HasValue && fechaFin.HasValue)
            {
                query = query.Where(d => d.Venta.Fecha >= fechaInicio && d.Venta.Fecha <= fechaFin);
            }

            var ventasProductos = await query
                .GroupBy(d => new
                {
                    Producto = d.Inventariolampara.Receta.Nombrelampara,
                    RecetaId = d.Inventariolampara.Receta.Id
                })
                .Select(g => new
                {
                    Id = g.Key.RecetaId,
                    Producto = g.Key.Producto,
                    NumeroDeVentas = g.Count(),
                    TotalRecaudado = g.Sum(d => d.PrecioUnitario * d.Cantidad)
                })
                .OrderByDescending(p => p.TotalRecaudado)
                .ToListAsync();

            return Ok(ventasProductos);
        }

        [HttpGet("VentasProductoPeriodos")]
        public async Task<IActionResult> GetVentasPorProductoPeriodo(DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            var query = from venta in _context.Venta
                        join detalleventa in _context.Detalleventa on venta.Id equals detalleventa.VentaId
                        join inventariolampara in _context.Inventariolamparas on detalleventa.InventariolamparaId equals inventariolampara.Id
                        join receta in _context.Receta on inventariolampara.RecetaId equals receta.Id
                        where !fechaInicio.HasValue || !fechaFin.HasValue || (venta.Fecha >= fechaInicio && venta.Fecha <= fechaFin)
                        group new { venta, detalleventa, receta, inventariolampara } by new
                        {
                            Año = venta.Fecha.Value.Year,
                            Mes = venta.Fecha.Value.Month,
                            Producto = receta.Nombrelampara,
                            LamparaId = inventariolampara.Id
                        } into g
                        select new
                        {
                            LamparaId = g.Key.LamparaId,
                            Año = g.Key.Año,
                            Mes = g.Key.Mes,
                            NumeroVentas = g.Count(),
                            Producto = g.Key.Producto
                        };

            var result = await query.OrderBy(r => r.Año)
                                     .ThenBy(r => r.Mes)
                                     .ThenBy(r => r.Producto)
                                     .ToListAsync();

            return Ok(result.Select(item => new
            {
                id = item.LamparaId,
                anio = item.Año,
                mes = item.Mes,
                producto = item.Producto,
                numeroDeVentas = item.NumeroVentas
            }));
        }

        [HttpGet("ExistenciasComponentes")]
        public async Task<ActionResult> GetExistenciasComponentes()
        {
            var existenciasComponentes = await _context.Componentes
                .Select(c => new
                {
                    Id = c.Id,
                    Componente = c.Nombre,
                    Existencia = _context.Inventariocomponentes
                                  .Where(i => i.ComponentesId == c.Id)
                                  .Sum(i => i.Cantidad)
                })
                .OrderBy(e => e.Existencia)
                .ToListAsync();

            return Ok(existenciasComponentes);
        }

        [HttpGet("ExistenciasLampara")]
        public async Task<ActionResult> GetExistenciasLampara()
        {
            var existenciasLampara = await _context.Receta
                .Select(r => new
                {
                    Id = r.Id,
                    ProductoTerminado = r.Nombrelampara,
                    Existencia = _context.Inventariolamparas
                                  .Where(i => i.RecetaId == r.Id)
                                  .Sum(i => i.Cantidad)
                })
                .OrderBy(e => e.Existencia)
                .ToListAsync();

            return Ok(existenciasLampara);
        }

        [HttpGet("VentasPeriodos")]
        public async Task<ActionResult> GetVentasPeriodos(DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            var query = _context.Venta.AsQueryable();

            if (fechaInicio.HasValue && fechaFin.HasValue)
            {
                query = query.Where(v => v.Fecha >= fechaInicio && v.Fecha <= fechaFin);
            }

            var ventasPeriodos = await query
                .GroupBy(v => new { v.Fecha.Value.Year, v.Fecha.Value.Month, v.Usuario.Id, v.Usuario.FullName })
                .Select(g => new
                {
                    Id = g.Key.Id,
                    Año = g.Key.Year,
                    Mes = g.Key.Month,
                    Cliente = g.Key.FullName,
                    NumeroDeCompras = g.Count()
                })
                .OrderByDescending(vp => vp.NumeroDeCompras)
                .ToListAsync();

            return Ok(ventasPeriodos);
        }

        [HttpGet("LamparasCliente")]
        public async Task<ActionResult> GetLamparasCliente(DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            var query = _context.Detalleventa.AsQueryable();

            if (fechaInicio.HasValue && fechaFin.HasValue)
            {
                query = query.Where(d => d.Venta.Fecha >= fechaInicio && d.Venta.Fecha <= fechaFin);
            }

            var lamparasCliente = await query
                .GroupBy(d => new
                {
                    d.Venta.Usuario.Id,
                    d.Venta.Usuario.FullName,
                    d.Inventariolampara.Receta.Nombrelampara
                })
                .Select(g => new
                {
                    Id = g.Key.Id,
                    Cliente = g.Key.FullName,
                    Producto = g.Key.Nombrelampara,
                    NumeroDeVentas = g.Count(),
                    TotalGastado = g.Sum(d => d.PrecioUnitario * d.Cantidad)
                })
                .OrderByDescending(p => p.TotalGastado)
                .ToListAsync();

            return Ok(lamparasCliente);
        }

        [HttpGet("MejorCliente")]
        public async Task<ActionResult> GetMejorCliente(DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            var query = _context.Detalleventa.AsQueryable();

            if (fechaInicio.HasValue && fechaFin.HasValue)
            {
                query = query.Where(d => d.Venta.Fecha >= fechaInicio && d.Venta.Fecha <= fechaFin);
            }

            var mejorCliente = await query
                .GroupBy(d => new { d.Venta.Usuario.Id, d.Venta.Usuario.FullName })
                .Select(g => new
                {
                    ClienteId = g.Key.Id,
                    MejorCliente = g.Key.FullName,
                    TotalGastado = g.Sum(d => d.PrecioUnitario * d.Cantidad)
                })
                .OrderByDescending(c => c.TotalGastado)
                .FirstOrDefaultAsync();

            if (mejorCliente != null)
            {
                return Ok(mejorCliente);
            }
            else
            {
                return BadRequest(new AuthResponseDTO { IsSuccess = false, Message = "Cliente no encontrado" });
            }
        }


    }

}
