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
        public async Task<ActionResult> GetVentasProductos()
        {
            var ventasProductos = await _context.Detalleventa
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
        public async Task<IActionResult> GetVentasPorProductoPeriodo()
        {
            var query = from venta in _context.Venta
                        join detalleventa in _context.Detalleventa on venta.Id equals detalleventa.VentaId
                        join inventariolampara in _context.Inventariolamparas on detalleventa.InventariolamparaId equals inventariolampara.Id
                        join receta in _context.Receta on inventariolampara.RecetaId equals receta.Id
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

            var response = result.Select(item => new
            {
                id = item.LamparaId,
                anio = item.Año,
                mes = item.Mes,
                producto = item.Producto,
                numeroDeVentas = item.NumeroVentas
            }).ToList();

            return Ok(response);
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
        public async Task<ActionResult> GetVentasPeriodos()
        {
            var ventasPeriodos = await _context.Venta
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
        public async Task<ActionResult> GetLamparasCliente()
        {
            var lamparasCliente = await _context.Detalleventa
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
        public async Task<ActionResult> GetMejorCliente()
        {
            var mejorCliente = await _context.Detalleventa
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
