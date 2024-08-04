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

        

        [HttpGet("componentesComprados")]
        public async Task<ActionResult> GetComponentesComprados()
        {
            var componentesComprados = await _baseDatos.ComponentesComprados.ToListAsync();

            return Ok(componentesComprados);
        }

        [HttpGet("ComponentesUsados")]
        public async Task<ActionResult> GetComponentesUsados()
        {
            var componentesUsados = await _baseDatos.ComponentesUsados.ToListAsync();

            return Ok(componentesUsados);
        }

        [HttpGet("ComprasMes")]
        public async Task<ActionResult> GetComprasMes()
        {
            var comprasMes = await _baseDatos.ComprasMes.ToListAsync();

            return Ok(comprasMes);
        }

        [HttpGet("ComprasProveedor")]
        public async Task<ActionResult> GetComprasProveedor()
        {
            var comprasProveedor = await _baseDatos.ComprasProveedors.ToListAsync();

            return Ok(comprasProveedor);
        }

        [HttpGet("MasVendidos")]
        public async Task<ActionResult> GetMasVendidos()
        {
            var masVendidos = await _baseDatos.MasVendidos.ToListAsync();

            return Ok(masVendidos);
        }

        [HttpGet("MermaProveedor")]
        public async Task<ActionResult> GetMermaProveedor()
        {
            var mermaProveedor = await _baseDatos.MermaProveedors.ToListAsync();

            return Ok(mermaProveedor);
        }

        [HttpGet("VentasMes")]
        public async Task<ActionResult> GetVentasMes()
        {
            var mermaProveedor = await _baseDatos.MermaProveedors.ToListAsync();

            return Ok(mermaProveedor);
        }


    }
}
