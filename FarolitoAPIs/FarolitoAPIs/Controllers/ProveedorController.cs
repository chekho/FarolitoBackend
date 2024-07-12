using FarolitoAPIs.Models;
using FarolitoAPIs.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


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

        [HttpGet]
        [Route("proveedores")]
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

            return Ok(listaTareas);
        }
        //POST documentado porque esta complicao
        [HttpPost]
        [Route("regproveedores")]
        public async Task<IActionResult> AgregarProveedor([FromBody] NuevoProveedorDTO nuevoProveedor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Crear una instancia de Proveedor y mapear los datos desde el DTO
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

            // Obtener los componentes existentes según los IDs proporcionados en el DTO
            var componentes = await _baseDatos.Componentes
                .Where(c => nuevoProveedor.Productos.Select(p => p.Id).Contains(c.Id))
                .ToListAsync();

            // Asociar los componentes al proveedor
            foreach (var componente in componentes)
            {
                proveedor.Productoproveedors.Add(new Productoproveedor
                {
                    ComponentesId = componente.Id,
                    Componentes = componente,
                    Proveedor = proveedor
                });
            }

            // Guardar el proveedor en la base de datos
            _baseDatos.Proveedors.Add(proveedor);
            await _baseDatos.SaveChangesAsync();

            return Ok(proveedor);
        }

        [HttpPut]
        [Route("editproveedores/{id}")]
        public async Task<IActionResult> EditarProveedor(int id, [FromBody] NuevoProveedorDTO proveedorActualizado)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var proveedorExistente = await _baseDatos.Proveedors
                .Include(p => p.Productoproveedors)
                .ThenInclude(pp => pp.Componentes)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (proveedorExistente == null)
            {
                return NotFound("Proveedor no encontrado");
            }

            proveedorExistente.NombreEmpresa = proveedorActualizado.NombreEmpresa;
            proveedorExistente.Dirección = proveedorActualizado.Dirección;
            proveedorExistente.Teléfono = proveedorActualizado.Teléfono;
            proveedorExistente.NombreAtiende = proveedorActualizado.NombreAtiende;
            proveedorExistente.ApellidoM = proveedorActualizado.ApellidoM;
            proveedorExistente.ApellidoP = proveedorActualizado.ApellidoP;
            proveedorExistente.Estatus = proveedorActualizado.Estatus;

            var nuevosComponentes = await _baseDatos.Componentes
                .Where(c => proveedorActualizado.Productos.Select(p => p.Id).Contains(c.Id))
                .ToListAsync();

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

            return Ok(proveedorExistente);
        }

        [HttpPatch]
        [Route("estatusproveedores/{id}")]
        public async Task<IActionResult> ActualizarEstatusProveedor(int id, [FromBody] ProveedorEstatusDTO estatusActualizado)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var proveedorExistente = await _baseDatos.Proveedors.FirstOrDefaultAsync(p => p.Id == id);

            if (proveedorExistente == null)
            {
                return NotFound("Proveedor no encontrado");
            }

            proveedorExistente.Estatus = estatusActualizado.Estatus;

            await _baseDatos.SaveChangesAsync();

            return Ok(proveedorExistente);
        }
    }
}
