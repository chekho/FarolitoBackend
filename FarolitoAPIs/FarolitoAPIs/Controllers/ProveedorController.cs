using FarolitoAPIs.Models;
using FarolitoAPIs.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using FarolitoAPIs.Data;
using Serilog;
using Microsoft.DotNet.Scaffolding.Shared;
using System.Data;
using System.Security.Claims;

namespace FarolitoAPIs.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProveedorController : ControllerBase
    {
        private readonly FarolitoDbContext _baseDatos;
        public ProveedorController(FarolitoDbContext baseDatos)
        {
            _baseDatos = baseDatos;
        }

        //[Authorize(Roles = "Administrador,Almacen")]
        [HttpGet("proveedores")]
        public async Task<IActionResult> xd()
        {
            //Log.Information("Fetching providers from the database.");

            var listaTareas = await _baseDatos.Proveedors
                .Include(p => p.Productoproveedors)
                .ThenInclude(pp => pp.Componentes)
                .Select(p => new
                {
                    Id = p.Id,
                    NombreEmpresa = p.NombreEmpresa,
                    Direccion = p.Direccion,
                    Telefono = p.Telefono,
                    NombreAtiende = p.NombreAtiende,
                    ApellidoM = p.ApellidoM,
                    ApellidoP = p.ApellidoP,
                    Estatus = p.Estatus,
                    productos = p.Productoproveedors.Select(pp => new
                    {
                        Id = pp.Componentes.Id,
                        Nombre = pp.Componentes.Nombre
                    }).ToList()
                })
                .ToListAsync();

            if (listaTareas == null || !listaTareas.Any())
            {
                Log.Warning("No providers found in the database.");

                return NotFound(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "No se encontraron proveedores"
                });
            }
            //Log.Information("Successfully retrieved {Count} providers.", listaTareas.Count);

            return Ok(listaTareas);
        }

        //POST documentado porque esta complicao
        //[Authorize(Roles = "Administrador,Almacen")]
        [HttpPost("regproveedores")]
        public async Task<IActionResult> AgregarProveedor([FromBody] NuevoProveedorDTO nuevoProveedor)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                Log.Warning("Unauthorized access attempt detected. No user ID found in token");
                return Unauthorized(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Usuario no autorizado"
                });
            }

            //Log.Information("Attempting to add a new provider.");

            if (!ModelState.IsValid)
            {
                Log.Warning("Invalid model state for provider addition.");

                return BadRequest(new AuthResponseDTO {
                    IsSuccess = false,
                    Message = "El modelo es invalido"
                });
            }

            var proveedor = new Proveedor
            {
                NombreEmpresa = nuevoProveedor.NombreEmpresa,
                Direccion = nuevoProveedor.Direccion,
                Telefono = nuevoProveedor.Telefono,
                NombreAtiende = nuevoProveedor.NombreAtiende,
                ApellidoM = nuevoProveedor.ApellidoM,
                ApellidoP = nuevoProveedor.ApellidoP,
                Estatus = nuevoProveedor.Estatus
            };

            //Log.Information("Fetching components for provider association.");

            var componentes = await _baseDatos.Componentes
                .Where(c => nuevoProveedor.Productos.Select(p => p.Id).Contains(c.Id))
                .ToListAsync();

            if (!componentes.Any())
            {
                Log.Warning("No valid components found to associate with the provider.");

                return NotFound(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "No se encontraron componentes válidos para asociar al proveedor"
                });
            }

            foreach (var componente in componentes)
            {
                proveedor.Productoproveedors.Add(new Productoproveedor
                {
                    ComponentesId = componente.Id,
                    Componentes = componente,
                    Proveedor = proveedor
                });
            }

            //Log.Information("Adding new provider to the database.");
            _baseDatos.Proveedors.Add(proveedor);
            await _baseDatos.SaveChangesAsync();

            Log.Information("Successfully added provider: {CompanyName}", proveedor.NombreEmpresa);

            // LOG BD agregar Proveedor
            LogDTO logDTO = new LogDTO
            {
                Cambio = "Proveedor agregado: " + nuevoProveedor.NombreEmpresa,
                Modulo = "Proveedores",
                UsuarioId = userId
            };

            LogsController lc = new LogsController(_baseDatos);
            AuthResponseDTO logVer = lc.AddLog(logDTO);

            if (logVer.IsSuccess)
            {
                return Ok(proveedor);
            }
            else
            {
                return BadRequest(logVer);
            }
        }

        //[Authorize(Roles = "Administrador,Almacen")]
        [HttpPut("editproveedores")]
        public async Task<IActionResult> EditarProveedor([FromBody] NuevoProveedorDTO proveedorActualizado)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                Log.Warning("Unauthorized access attempt detected. No user ID found in token");
                return Unauthorized(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Usuario no autorizado"
                });
            }

            //Log.Information("Attempting to update provider with ID: {ProviderId}", proveedorActualizado.Id);

            if (!ModelState.IsValid)
            {
                Log.Warning("Invalid model state for provider update.");

                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "El modelo es inválido"
                });
            }

            var proveedorExistente = await _baseDatos.Proveedors
                .Include(p => p.Productoproveedors)
                .ThenInclude(pp => pp.Componentes)
                .FirstOrDefaultAsync(p => p.Id == proveedorActualizado.Id);

            if (proveedorExistente == null)
            {
                Log.Warning("Provider not found with ID: {ProviderId}", proveedorActualizado.Id);

                return NotFound(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Proveedor no encontrado"
                });
            }

            proveedorExistente.NombreEmpresa = proveedorActualizado.NombreEmpresa;
            proveedorExistente.Direccion = proveedorActualizado.Direccion;
            proveedorExistente.Telefono = proveedorActualizado.Telefono;
            proveedorExistente.NombreAtiende = proveedorActualizado.NombreAtiende;
            proveedorExistente.ApellidoM = proveedorActualizado.ApellidoM;
            proveedorExistente.ApellidoP = proveedorActualizado.ApellidoP;
            proveedorExistente.Estatus = true;

            //Log.Information("Fetching new components for provider update.");

            var nuevosComponentes = await _baseDatos.Componentes
                .Where(c => proveedorActualizado.Productos.Select(p => p.Id).Contains(c.Id))
                .ToListAsync();

            if (!nuevosComponentes.Any())
            {
                Log.Warning("No valid components found to associate with provider ID: {ProviderId}", proveedorActualizado.Id);

                return NotFound(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "No se encontraron componentes válidos para asociar al proveedor"
                });
            }

            //Log.Information("Removing old components associated with provider ID: {ProviderId}", proveedorActualizado.Id);

            _baseDatos.Productoproveedors.RemoveRange(proveedorExistente.Productoproveedors);
            proveedorExistente.Productoproveedors.Clear();

            foreach (var componente in nuevosComponentes)
            {
                proveedorExistente.Productoproveedors.Add(new Productoproveedor
                {
                    ComponentesId = componente.Id,
                    Componentes = componente,
                    ProveedorId = proveedorExistente.Id,
                    Proveedor = proveedorExistente
                });
            }
            //Log.Information("Saving changes for provider ID: {ProviderId}", proveedorActualizado.Id);

            await _baseDatos.SaveChangesAsync();

            //Log.Information("Successfully updated provider ID: {ProviderId}", proveedorActualizado.Id);

            // LOG BD Actualización Proveedor
            LogDTO logDTO = new LogDTO
            {
                Cambio = "Actualización de proveedor " + proveedorActualizado.NombreEmpresa,
                Modulo = "Proveedores",
                UsuarioId = userId
            };

            LogsController lc = new LogsController(_baseDatos);
            AuthResponseDTO logVer = lc.AddLog(logDTO);

            if (logVer.IsSuccess)
            {
                return Ok(new AuthResponseDTO
                {
                    IsSuccess = true,
                    Message = "Proveedor actualizado exitosamente"
                });
            }
            else
            {
                return BadRequest(logVer);
            }

            
        }

        //[Authorize(Roles = "Administrador,Almacen")]
        [HttpPatch("estatusproveedores")]
        public async Task<IActionResult> ActualizarEstatusProveedor([FromBody] ProveedorEstatusDTO estatusActualizado)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                Log.Warning("Unauthorized access attempt detected. No user ID found in token");
                return Unauthorized(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Usuario no autorizado"
                });
            }

            //Log.Information("Attempting to update status for provider with ID: {ProviderId}", estatusActualizado.Id);

            if (!ModelState.IsValid)
            {
                Log.Warning("Invalid model state for provider status update.");

                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "El modelo es inválido"
                });
            }

            var proveedorExistente = await _baseDatos.Proveedors.FirstOrDefaultAsync(p => p.Id == estatusActualizado.Id);

            if (proveedorExistente == null)
            {
                Log.Warning("Provider not found with ID: {ProviderId}", estatusActualizado.Id);

                return NotFound(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Proveedor no encontrado"
                });
            }

            //Log.Information("Updating status for provider ID: {ProviderId} to status: {Status}", estatusActualizado.Id, estatusActualizado.Estatus);
            proveedorExistente.Estatus = estatusActualizado.Estatus;

            await _baseDatos.SaveChangesAsync();

            // LOG BD actualización Proveedor
            LogDTO logDTO = new LogDTO
            {
                Cambio = "Actualización de estatus de proveedor " + proveedorExistente.NombreEmpresa,
                Modulo = "Proveedores",
                UsuarioId = userId
            };

            LogsController lc = new LogsController(_baseDatos);
            AuthResponseDTO logVer = lc.AddLog(logDTO);

            if (logVer.IsSuccess)
            {
                return Ok(new AuthResponseDTO
                {
                    IsSuccess = true,
                    Message = "Estatus del proveedor actualizado exitosamente"
                });
            }
            else
            {
                return BadRequest(logVer);
            }
            
        }

    }
}
