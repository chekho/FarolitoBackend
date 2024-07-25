using FarolitoAPIs.Data;
using FarolitoAPIs.DTOs;
using FarolitoAPIs.Models;
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
                        .ThenInclude(ic => ic.Componentes) 
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
                        .Select(ic => ic.Componentes.Nombre)
                        .FirstOrDefault() 
                }).ToList()
            }).ToList();

            return Ok(comprasDTO);
        }

        [HttpGet("comprasUsuario")]
        public async Task<IActionResult> ObtenerComprasUsuario()
        {
            var usuarioId = User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(usuarioId))
            {
                return Unauthorized(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Usuario no autenticado"
                });
            }

            var compras = await _baseDatos.Compras
                .Where(c => c.UsuarioId == usuarioId)
                .Include(c => c.Detallecompras)
                    .ThenInclude(dc => dc.Inventariocomponentes)
                        .ThenInclude(ic => ic.Componentes)
                .Include(c => c.Usuario)
                .ToListAsync();

            if (compras == null || !compras.Any())
            {
                return NotFound(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "No se encontraron compras para el usuario"
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
                        .Select(ic => ic.Componentes.Nombre)
                        .FirstOrDefault()
                }).ToList()
            }).ToList();

            return Ok(comprasDTO);
        }

        [Authorize]
        [HttpPost("compras")]
        public async Task<IActionResult> AgregarCompra([FromBody] AgregarCompraDTO nuevaCompra)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Datos del modelo no válidos"
                });
            }

            var usuarioId = User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(usuarioId))
            {
                return Unauthorized(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Usuario no autenticado"
                });
            }

            var compra = new Compra
            {
                Fecha = nuevaCompra.Fecha,
                UsuarioId = usuarioId
            };

            _baseDatos.Compras.Add(compra);
            await _baseDatos.SaveChangesAsync();

            foreach (var detalleDTO in nuevaCompra.Detalles)
            {
                bool proveedorTieneComponente = await _baseDatos.Productoproveedors
                    .AnyAsync(pp => pp.ProveedorId == nuevaCompra.ProveedorId && pp.ComponentesId == detalleDTO.ComponentesId);

                if (!proveedorTieneComponente)
                {
                    return BadRequest(new AuthResponseDTO
                    {
                        IsSuccess = false,
                        Message = $"El proveedor con ID {nuevaCompra.ProveedorId} no tiene el componente con ID {detalleDTO.ComponentesId}"
                    });
                }
            }

            foreach (var detalleDTO in nuevaCompra.Detalles)
            {
                var detallecompra = new Detallecompra
                {
                    Cantidad = detalleDTO.Cantidad,
                    Costo = detalleDTO.Costo,
                    CompraId = compra.Id
                };

                _baseDatos.Detallecompras.Add(detallecompra);
                await _baseDatos.SaveChangesAsync(); 

                var inventario = new Inventariocomponente
                {
                    ComponentesId = detalleDTO.ComponentesId,
                    Cantidad = detalleDTO.Cantidad,
                    DetallecompraId = detallecompra.Id,
                    ProveedorId = nuevaCompra.ProveedorId 
                };

                _baseDatos.Inventariocomponentes.Add(inventario);
            }

            await _baseDatos.SaveChangesAsync();

            return Ok(new AuthResponseDTO
            {
                IsSuccess = true,
                Message = "Compra agregada exitosamente"
            });
        }



    }
}
