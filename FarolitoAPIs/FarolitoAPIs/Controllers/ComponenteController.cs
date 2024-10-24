using FarolitoAPIs.Data;
using FarolitoAPIs.DTOs;
using FarolitoAPIs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace FarolitoAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComponenteController : ControllerBase
    {
        private readonly FarolitoDbContext _baseDatos;
        public ComponenteController(FarolitoDbContext baseDatos)
        {
            _baseDatos = baseDatos;
        }
        //[Authorize(Roles = "Administrador,Almacen")]
        [HttpGet("componentes")]
        public async Task<IActionResult> ListaComponentes()
        {
            //Log.Information("Request received to fetch the list of components");
            
            var listaComponentes = await _baseDatos.Componentes.ToListAsync();

            if (listaComponentes == null || !listaComponentes.Any())
            {
                Log.Warning("No components found in the database");
                return NotFound(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "No se encontraron componentes"
                });
            }

            //Log.Information("Found {ComponentCount} components", listaComponentes.Count);
            return Ok(listaComponentes);
        }

        //[Authorize(Roles = "Administrador,Almacen")]
        [HttpPost("componente")]
        public async Task<IActionResult> AgregarComponente([FromBody] ComponenteDTO nuevoComponente)
        {

            //Log.Information("Request received to add a new component");
            if (!ModelState.IsValid)
            {

                Log.Warning("Invalid model state detected for the new component. Model: {@ComponenteDTO}", nuevoComponente);
                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "El modelo es inválido"
                });
            }

            var componente = new Componente
            {
                Nombre = nuevoComponente.Nombre,
                estatus = true
            };

            //Log.Information("Adding new component: {@Componente}", componente);

            _baseDatos.Componentes.Add(componente);
            await _baseDatos.SaveChangesAsync();

            //Log.Information("Component added successfuly with ID: {ComponentId}", componente.Id);
            return Ok(new AuthResponseDTO
            {
                IsSuccess = true,
                Message = "Componente agregado exitosamente"
            });
        }

        [AllowAnonymous]
        [HttpGet("proveedorComponentes")]
        public async Task<IActionResult> ProveedorComponentes([FromQuery] int idProveedor)
        {

            //Log.Information("Request received to fetch components for provider with ID: {IdProveedor}", idProveedor);
            var listaComponentes = await _baseDatos.Productoproveedors.Include(p => p.Proveedor).Include(p=>p.Componentes).Where(p=>p.ProveedorId == idProveedor).ToListAsync();

            if (listaComponentes == null || !listaComponentes.Any())
            {
                Log.Warning("No components found for provider with ID: {IdProveedor}", idProveedor);
                return NotFound(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "No se encontraron componentes"
                });
            }

            //Log.Information("Components successfuly retrieved for provider with ID: {IdProveedor}. Total components: {Count}", idProveedor, listaComponentes.Count);

            return Ok(listaComponentes.Select(c=> new
            {
                c.ProveedorId,
                c.Proveedor.NombreEmpresa,
                c.Componentes.Nombre
            }));
        }

        //[Authorize(Roles = "Administrador,Almacen")]
        [HttpPut("componente")]
        public async Task<IActionResult> EditarComponente([FromBody] ComponenteDTO editarComponente)
        {

            //Log.Information("Request received to edit component with ID: {ComponenteId}", editarComponente.Id);
            if (!ModelState.IsValid)
            {
                Log.Warning("Invalid model received for editing component with ID: {ComponenteId}", editarComponente.Id);
                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "El modelo es inválido"
                });
            }

            var componenteExistente = await _baseDatos.Componentes
                .FirstOrDefaultAsync(c => c.Id == editarComponente.Id);

            if (componenteExistente == null)
            {
                Log.Warning("Component with ID: {ComponentId} not found", editarComponente.Id);
                return NotFound(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Componente no encontrado"
                });
            }

            //Log.Information("Updating component with ID: {ComponenteId}", editarComponente.Id);
            componenteExistente.Nombre = editarComponente.Nombre;
            componenteExistente.estatus = true;

            await _baseDatos.SaveChangesAsync();


            //Log.Information("Component with ID: {ComponentId} updated successfuly", editarComponente.Id);
            return Ok(new AuthResponseDTO
            {
                IsSuccess = true,
                Message = "Componente actualizado exitosamente"
            });
        }

        //[Authorize(Roles = "Administrador,Almacen")]
        [HttpPatch("componente")]
        public async Task<IActionResult> ActualizarEstatusComponente([FromBody] PatchComponenteDTO estatusDTO)
        {
            //Log.Information("Request received to update status for component with ID: {ComponenteId}", estatusDTO.Id);
            if (!ModelState.IsValid)
            {
                Log.Warning("Invalid model received for updating status of component with ID: {ComponenteId}", estatusDTO.Id);
                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "El modelo es inválido"
                });
            }

            var componenteExistente = await _baseDatos.Componentes
                .FirstOrDefaultAsync(c => c.Id == estatusDTO.Id);

            if (componenteExistente == null)
            {
                Log.Warning("Component with ID: {ComponenteId} not found", estatusDTO.Id);
                return NotFound(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Componente no encontrado"
                });
            }

            //Log.Information("Updating status for component with ID: {ComponenteId} to {Estatus}", estatusDTO.Id, estatusDTO.estatus);
            componenteExistente.estatus = estatusDTO.estatus;

            await _baseDatos.SaveChangesAsync();

            return Ok(new AuthResponseDTO
            {
                IsSuccess = true,
                Message = "Estatus del componente actualizado exitosamente"
            });
        }
    }
}
