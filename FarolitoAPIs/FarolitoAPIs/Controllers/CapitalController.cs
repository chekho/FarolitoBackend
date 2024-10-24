using FarolitoAPIs.Data;
using FarolitoAPIs.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using System.Security.Claims;
using Serilog;

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

            //Log.Information("Request to calculate total ventas and compras received");
            DateTime? startDateTime = rango.StartDate != null ? DateTime.Parse(rango.StartDate) : null;
            DateTime? endDateTime = rango.EndDate != null ? DateTime.Parse(rango.EndDate).AddDays(1).AddTicks(-1) : null;
            Log.Information("Start date: {StartDate}, End date: {EndDate}", startDateTime, endDateTime);
            var ventasQuery = _baseDatos.Venta.AsQueryable();

            if (startDateTime.HasValue && endDateTime.HasValue)
            {
                //Log.Information("Filtering ventas between {StartDate} and {EndDate}", startDateTime, endDateTime);
                ventasQuery = ventasQuery.Where(v => v.Fecha >= startDateTime && v.Fecha <= endDateTime);
            }

            var totalVentas = await ventasQuery
                .SelectMany(v => v.Detalleventa)
                .SumAsync(dv => (dv.Cantidad ?? 0) * (dv.PrecioUnitario ?? 0));

            //Log.Information("Total ventas calculated: {TotalVentas}", totalVentas);


            var comprasQuery = _baseDatos.Compras.AsQueryable();

            if (startDateTime.HasValue && endDateTime.HasValue)
            {
                var startDateOnly = DateOnly.FromDateTime(startDateTime.Value);
                var endDateOnly = DateOnly.FromDateTime(endDateTime.Value);

                //Log.Information("Filtering compras between {StarDateOnly} and {EndDateOnly}", startDateOnly, endDateOnly);
                comprasQuery = comprasQuery.Where(c => c.Fecha >= startDateOnly && c.Fecha <= endDateOnly);
            }

            var totalCompras = await comprasQuery
                .SelectMany(c => c.Detallecompras)
                .SumAsync(dc => dc.Costo ?? 0);

            //Log.Information("Total compras calculated: {TotalCompras}", totalCompras);
            var ganancia = totalVentas - totalCompras;

            string estado = ganancia < 0 ? "En pérdidas" : "En ganancia";

            //Log.Information("Ganancia calculated: {Ganancia}, Estado: {Estado}", ganancia, estado);
            return Ok(new
            {
                TotalVentas = totalVentas,
                TotalCompras = totalCompras,
                Ganancia = ganancia,
                Estado = estado
            });
        }
    }
}
