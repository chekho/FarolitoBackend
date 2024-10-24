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
        private readonly FarolitoDbContext _baseDatos;
        public DashboardController(FarolitoDbContext baseDatos)
        {
            _baseDatos = baseDatos;
        }

        [HttpGet("VentasProductos")]
        public async Task<ActionResult> GetVentasProductos()
        {

            //Log.Information("Request received to fetch product sales");
            var ventasProductos = await _baseDatos.VentasProductos.ToListAsync();

            //Log.Information("Fetched {Count} product sales records successfuly", ventasProductos.Count);
            return Ok(ventasProductos);
        }

        [HttpGet("VentasProductoPeriodos")]
        public async Task<ActionResult> GetVentasProductoPeriodos()
        {
            //Log.Information("Request received to fetch sales data for product periods");
            var ventasProductoPeriodos = await _baseDatos.VentasProductoPeriodos.ToListAsync();

            //Log.Information("Fetched {Count} records of sales data for product periods successfully", ventasProductoPeriodos.Count);
            return Ok(ventasProductoPeriodos);
        }

        [HttpGet("ExistenciasComponentes")]
        public async Task<ActionResult> GetExistenciasComponentes()
        {
            //Log.Information("Request received to fetch component stock data.");

            var existenciasComponentes = await _baseDatos.ExistenciasComponentes.ToListAsync();

            //Log.Information("Fetched {Count} records of component stock data successfully.", existenciasComponentes.Count);

            return Ok(existenciasComponentes);
        }

        [HttpGet("ExistenciasLampara")]
        public async Task<ActionResult> GetExistenciasLampara()
        {
            //Log.Information("Request received to fetch lamp stock data.");

            var existenciasLampara = await _baseDatos.ExistenciasLampara.ToListAsync();

            //Log.Information("Fetched {Count} records of lamp stock data successfully.", existenciasLampara.Count);

            return Ok(existenciasLampara);
        }

        [HttpGet("VentasPeriodos")]
        public async Task<ActionResult> GetVentasPeriodos()
        {
            //Log.Information("Request received to fetch sales periods data.");

            var ventasPeriodos = await _baseDatos.VentasPeriodos.ToListAsync();

            //Log.Information("Fetched {Count} records of sales periods data successfully.", ventasPeriodos.Count);

            return Ok(ventasPeriodos);
        }

        [HttpGet("LamparasCliente")]
        public async Task<ActionResult> GetLamparasCliente()
        {
            //Log.Information("Request received to fetch customer lamp data.");

            var lamparasCliente = await _baseDatos.LamparasCliente.ToListAsync();

            //Log.Information("Fetched {Count} records of customer lamp data successfully.", lamparasCliente.Count);

            return Ok(lamparasCliente);
        }

        [HttpGet("MejorCliente")]
        public async Task<ActionResult> GetMejorCliente()
        {
            //Log.Information("Request received to fetch the best customer.");

            var mejorCliente = await _baseDatos.MejorCliente.FirstOrDefaultAsync();

            if (mejorCliente != null)
            {
                //Log.Information("Best customer data retrieved successfully.");

                return Ok(mejorCliente);
            }
            else {
                Log.Warning("No customer data found for the best customer.");
                return BadRequest(new AuthResponseDTO { IsSuccess = false, Message = "Cliente no encontrado" });
            } 
        }
    }
}
