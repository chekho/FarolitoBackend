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
    public class CompraController : ControllerBase
    {
        private readonly FarolitoDbContext _baseDatos;
        public CompraController(FarolitoDbContext baseDatos)
        {
            _baseDatos = baseDatos;
        }


        [HttpGet("compras")]
        public async Task<IActionResult> ObtenerCompras()
        {
            var compras = await _baseDatos.Compras
                .Include(c => c.Detallecompras)
                    .ThenInclude(dc => dc.Inventariocomponentes)
                        .ThenInclude(ic => ic.Componentes) // Incluye Componentes a través de Inventariocomponentes
                .Include(c => c.Usuario)
                .ToListAsync();

            if (compras == null || !compras.Any())
            {
                return NotFound(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "No se encontraron compras"
                });
            }

            var comprasDTO = compras.Select(c => new CompraDTO
            {
                Id = c.Id,
                Fecha = c.Fecha,
                UsuarioNombre = c.Usuario.UserName,
                Detalles = c.Detallecompras.Select(dc => new DetalleCompraDTO
                {
                    Id = dc.Id,
                    Cantidad = dc.Cantidad,
                    Costo = dc.Costo,
                    NombreComponente = dc.Inventariocomponentes
                        .Select(ic => ic.Componentes.Nombre) // Obtiene el nombre del componente
                        .FirstOrDefault() // Asume que hay un único componente por detalle
                }).ToList()
            }).ToList();

            return Ok(comprasDTO);
        }


    }
}
