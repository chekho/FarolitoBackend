using FarolitoAPIs.Models;
using FarolitoAPIs.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using FarolitoAPIs.Data;


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

        [Authorize(Roles = "Administrador,Almacen")]
        [HttpGet("proveedores")]
        public async Task<IActionResult> xd()
        {
            var listaTareas = await _baseDatos.Proveedors
                .Include(p => p.Productoproveedors)
                .ThenInclude(pp => pp.Componentes)
                .Select(p => new
                {
                    Id = p.Id,
                    NombreEmpresa = p.NombreEmpresa,
                    Dirección = p.Dirección,
                    Teléfono = p.Teléfono,
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
                return NotFound(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "No se encontraron proveedores"
                });
            }

            return Ok(listaTareas);
        }

        //POST documentado porque esta complicao
        [Authorize(Roles = "Administrador,Almacen")]
        [HttpPost("regproveedores")]
        public async Task<IActionResult> AgregarProveedor([FromBody] NuevoProveedorDTO nuevoProveedor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResponseDTO {
                    IsSuccess = false,
                    Message = "El modelo es invalido"
                });
            }

            var proveedor = new Proveedor
            {
                NombreEmpresa = nuevoProveedor.NombreEmpresa,
                Dirección = nuevoProveedor.Dirección,
                Teléfono = nuevoProveedor.Teléfono,
                NombreAtiende = nuevoProveedor.NombreAtiende,
                ApellidoM = nuevoProveedor.ApellidoM,
                ApellidoP = nuevoProveedor.ApellidoP,
                Estatus = nuevoProveedor.Estatus
            };

            var componentes = await _baseDatos.Componentes
                .Where(c => nuevoProveedor.Productos.Select(p => p.Id).Contains(c.Id))
                .ToListAsync();

            if (!componentes.Any())
            {
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

            _baseDatos.Proveedors.Add(proveedor);
            await _baseDatos.SaveChangesAsync();

            return Ok(proveedor);
        }

        [Authorize(Roles = "Administrador,Almacen")]
        [HttpPut("editproveedores")]
        public async Task<IActionResult> EditarProveedor([FromBody] NuevoProveedorDTO proveedorActualizado)
        {
            if (!ModelState.IsValid)
            {
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
                return NotFound(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Proveedor no encontrado"
                });
            }

            proveedorExistente.NombreEmpresa = proveedorActualizado.NombreEmpresa;
            proveedorExistente.Dirección = proveedorActualizado.Dirección;
            proveedorExistente.Teléfono = proveedorActualizado.Teléfono;
            proveedorExistente.NombreAtiende = proveedorActualizado.NombreAtiende;
            proveedorExistente.ApellidoM = proveedorActualizado.ApellidoM;
            proveedorExistente.ApellidoP = proveedorActualizado.ApellidoP;
            proveedorExistente.Estatus = true;

            var nuevosComponentes = await _baseDatos.Componentes
                .Where(c => proveedorActualizado.Productos.Select(p => p.Id).Contains(c.Id))
                .ToListAsync();

            if (!nuevosComponentes.Any())
            {
                return NotFound(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "No se encontraron componentes válidos para asociar al proveedor"
                });
            }

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

            await _baseDatos.SaveChangesAsync();

            return Ok(new AuthResponseDTO
            {
                IsSuccess = true,
                Message = "Proveedor actualizado exitosamente"
            });
        }

        [Authorize(Roles = "Administrador,Almacen")]
        [HttpPatch("estatusproveedores")]
        public async Task<IActionResult> ActualizarEstatusProveedor([FromBody] ProveedorEstatusDTO estatusActualizado)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "El modelo es inválido"
                });
            }

            var proveedorExistente = await _baseDatos.Proveedors.FirstOrDefaultAsync(p => p.Id == estatusActualizado.Id);

            if (proveedorExistente == null)
            {
                return NotFound(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Proveedor no encontrado"
                });
            }

            proveedorExistente.Estatus = estatusActualizado.Estatus;

            await _baseDatos.SaveChangesAsync();

            return Ok(new AuthResponseDTO
            {
                IsSuccess = true,
                Message = "Estatus del proveedor actualizado exitosamente"
            });
        }

    }
}
