using FarolitoAPIs.Data;
using FarolitoAPIs.DTOs;
using FarolitoAPIs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var ventasProductos = await _baseDatos.VentasProductos.ToListAsync();

            return Ok(ventasProductos);
        }

        [HttpGet("VentasProductoPeriodos")]
        public async Task<ActionResult> GetVentasProductoPeriodos()
        {
            var ventasProductoPeriodos = await _baseDatos.VentasProductoPeriodos.ToListAsync();

            return Ok(ventasProductoPeriodos);
        }

        [HttpGet("ExistenciasComponentes")]
        public async Task<ActionResult> GetExistenciasComponentes()
        {
            var existenciasComponentes = await _baseDatos.ExistenciasComponentes.ToListAsync();

            return Ok(existenciasComponentes);
        }

        [HttpGet("ExistenciasLampara")]
        public async Task<ActionResult> GetExistenciasLampara()
        {
            var existenciasLampara = await _baseDatos.ExistenciasLampara.ToListAsync();

            return Ok(existenciasLampara);
        }

        [HttpGet("VentasPeriodos")]
        public async Task<ActionResult> GetVentasPeriodos()
        {
            var ventasPeriodos = await _baseDatos.VentasPeriodos.ToListAsync();

            return Ok(ventasPeriodos);
        }

        [HttpGet("LamparasCliente")]
        public async Task<ActionResult> GetLamparasCliente()
        {
            var lamparasCliente = await _baseDatos.LamparasCliente.ToListAsync();

            return Ok(lamparasCliente);
        }

        [HttpGet("MejorCliente")]
        public async Task<ActionResult> GetMejorCliente()
        {
            var mejorCliente = await _baseDatos.MejorCliente.FirstOrDefaultAsync();

            if (mejorCliente != null) return Ok(mejorCliente);
            else return BadRequest(new AuthResponseDTO {IsSuccess = false, Message = "Cliente no encontrado" });
        }
    }
}
