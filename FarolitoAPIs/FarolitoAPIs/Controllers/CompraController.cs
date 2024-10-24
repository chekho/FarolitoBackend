using FarolitoAPIs.Data;
using FarolitoAPIs.DTOs;
using FarolitoAPIs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Serilog;

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
            //Log.Information("Request received to obtain purchases");
            var compras = await _baseDatos.Compras
                .Include(c => c.Detallecompras)
                    .ThenInclude(dc => dc.Inventariocomponentes)
                        .ThenInclude(ic => ic.Componentes) 
                .Include(c => c.Usuario)
                .ToListAsync();

            if (compras == null || !compras.Any())
            {
                Log.Warning("No purchases found");
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

            //Log.Information("Successfuly retrieved {Count} purchases.", comprasDTO.Count);
            return Ok(comprasDTO);
        }

        [Authorize]
        [HttpGet("comprasUsuario")]
        public async Task<IActionResult> ObtenerComprasUsuario()
        {
            //Log.Information("Request received to obtain purchases for user");
            var usuarioId = User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(usuarioId))
            {
                Log.Warning("Unauthorized access attempt by an unauthenticated user");
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
                //Log.Information("No purchases found for user with ID: {UserId}", usuarioId);
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

            //Log.Information("Successfuly retrieved {Count} purchases for user ID: {UserId}", comprasDTO.Count, usuarioId);
            return Ok(comprasDTO);
        }

        [Authorize]
        [HttpPost("agregar-compras")]
        public async Task<IActionResult> AgregarCompra([FromBody] AgregarCompraDTO nuevaCompra)
        {
            //Log.Information("Request received to add a new purchase.");
            if (!ModelState.IsValid)
            {
                Log.Warning("Invalid model data received for new purchase");
                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Datos del modelo no válidos"
                });
            }

            var usuarioId = User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(usuarioId))
            {
                Log.Warning("Unauthorized access attempt by an unauthenticated user");
                return Unauthorized(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Usuario no autenticado"
                });
            }

            var compra = new Compra
            {
                Fecha = DateOnly.FromDateTime(DateTime.UtcNow.AddHours(-6)),
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
                    Log.Warning("The supplier with ID {ProviderId} does not have the component with ID {ComponentesId}", nuevaCompra.ProveedorId, detalleDTO.ComponentesId);
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
            //Log.Information("Purchase added successfuly with ID: {CompraId} by user ID: {UserId}", compra.Id, usuarioId);
            return Ok(new AuthResponseDTO
            {
                IsSuccess = true,
                Message = "Compra agregada exitosamente"
            });
        }



    }
}
