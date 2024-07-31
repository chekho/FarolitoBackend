using FarolitoAPIs.Data;
using FarolitoAPIs.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using System.Security.Claims;

namespace FarolitoAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CapitalController : ControllerBase
    {
        private readonly FarolitoDbContext _baseDatos;
        public CapitalController(FarolitoDbContext baseDatos)
        {
            _baseDatos = baseDatos;
        }

        [HttpGet("total")]
        public async Task<IActionResult> GetTotalVentasCompras([FromQuery] DateRangeRequestDTO rango)
        {
            DateTime? startDateTime = rango.StartDate != null ? DateTime.Parse(rango.StartDate) : null;
            DateTime? endDateTime = rango.EndDate != null ? DateTime.Parse(rango.EndDate).AddDays(1).AddTicks(-1) : null;

            var ventasQuery = _baseDatos.Venta.AsQueryable();

            if (startDateTime.HasValue && endDateTime.HasValue)
            {
                ventasQuery = ventasQuery.Where(v => v.Fecha >= startDateTime && v.Fecha <= endDateTime);
            }

            var totalVentas = await ventasQuery
                .SelectMany(v => v.Detalleventa)
                .SumAsync(dv => (dv.Cantidad ?? 0) * (dv.PrecioUnitario ?? 0));

            var comprasQuery = _baseDatos.Compras.AsQueryable();

            if (startDateTime.HasValue && endDateTime.HasValue)
            {
                var startDateOnly = DateOnly.FromDateTime(startDateTime.Value);
                var endDateOnly = DateOnly.FromDateTime(endDateTime.Value);

                comprasQuery = comprasQuery.Where(c => c.Fecha >= startDateOnly && c.Fecha <= endDateOnly);
            }

            var totalCompras = await comprasQuery
                .SelectMany(c => c.Detallecompras)
                .SumAsync(dc => dc.Costo ?? 0); 

            return Ok(new
            {
                TotalVentas = totalVentas,
                TotalCompras = totalCompras
            });
        }
    }
}
