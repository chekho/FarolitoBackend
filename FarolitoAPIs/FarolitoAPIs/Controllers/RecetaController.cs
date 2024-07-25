using FarolitoAPIs.Data;
using FarolitoAPIs.DTOs;
using FarolitoAPIs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FarolitoAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecetaController : ControllerBase
    {
        private readonly FarolitoDbContext _baseDatos;
        public RecetaController(FarolitoDbContext baseDatos)
        {
            _baseDatos = baseDatos;
        }

        //[Authorize(Roles = "Administrador,Produccion")]
        [HttpGet("recetas")]
        public async Task<IActionResult> ObtenerRecetas()
        {
            var recetas = await _baseDatos.Receta
                .Include(r => r.Componentesreceta)
                    .ThenInclude(cr => cr.Componentes)
                        .ThenInclude(c => c.Inventariocomponentes)
                            .ThenInclude(ic => ic.Detallecompra)
                .ToListAsync();

            if (recetas == null || !recetas.Any())
            {
                return NotFound(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "No se encontraron recetas"
                });
            }

            var recetasDTO = recetas.Select(r => {
                var componentesDTO = r.Componentesreceta.Select(cr => {
                    var precioUnitario = cr.Componentes.Inventariocomponentes.Any()
                        ? cr.Componentes.Inventariocomponentes.Average(ic => ic.Detallecompra.Costo.HasValue && ic.Detallecompra.Cantidad.HasValue && ic.Detallecompra.Cantidad != 0
                            ? (decimal)(ic.Detallecompra.Costo.Value / ic.Detallecompra.Cantidad.Value)
                            : 0)
                        : 0;

                    precioUnitario = Math.Round(precioUnitario, 2);

                    return new ComponenteRecetaDTO
                    {
                        Id = cr.Componentes.Id,
                        Nombre = cr.Componentes.Nombre,
                        Cantidad = cr.Cantidad,
                        Estatus = cr.Estatus,
                        PrecioUnitario = precioUnitario,
                        PrecioTotal = precioUnitario * (cr.Cantidad ?? 0)
                    };
                }).ToList();

                var costoProduccion = componentesDTO.Sum(c => c.PrecioTotal);

                return new RecetaDetalleDTO
                {
                    Id = r.Id,
                    Nombrelampara = r.Nombrelampara,
                    Estatus = r.Estatus,
                    CostoProduccion = costoProduccion,
                    Componentes = componentesDTO
                    
                };
            }).ToList();

            return Ok(recetasDTO);
        }




        //[Authorize(Roles = "Administrador,Produccion")]
        [HttpPost("recetas")]
        public async Task<IActionResult> AgregarReceta([FromBody] RecetaDetalle2DTO nuevaReceta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Datos del modelo no válidos"
                });
            }

            var receta = new Recetum
            {
                Nombrelampara = nuevaReceta.Nombrelampara,
                Estatus = true
            };

            var componentes = await _baseDatos.Componentes
                .Where(c => nuevaReceta.Componentes.Select(cr => cr.Id).Contains(c.Id))
                .ToListAsync();

            if (componentes.Count != nuevaReceta.Componentes.Count)
            {
                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Algunos componentes no se encontraron en la base de datos"
                });
            }

            foreach (var componente in componentes)
            {
                var cantidad = nuevaReceta.Componentes.First(cr => cr.Id == componente.Id).Cantidad;

                receta.Componentesreceta.Add(new Componentesrecetum
                {
                    ComponentesId = componente.Id,
                    Cantidad = cantidad,
                    Receta = receta,
                    Estatus = true,
                    Componentes = componente
                });
            }

            _baseDatos.Receta.Add(receta);
            await _baseDatos.SaveChangesAsync();

            return Ok(new AuthResponseDTO
            {
                IsSuccess = true,
                Message = "Receta agregada exitosamente"
            });
        }

        [HttpPut("recetas")]
        public async Task<IActionResult> EditarReceta([FromBody] RecetaDetalle2DTO recetaEditada)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Datos del modelo no válidos"
                });
            }

            var recetaExistente = await _baseDatos.Receta
                .Include(r => r.Componentesreceta)
                .FirstOrDefaultAsync(r => r.Id == recetaEditada.Id);

            if (recetaExistente == null)
            {
                return NotFound(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Receta no encontrada"
                });
            }

            recetaExistente.Nombrelampara = recetaEditada.Nombrelampara;
            recetaExistente.Estatus = recetaEditada.Estatus;

            _baseDatos.Componentesreceta.RemoveRange(recetaExistente.Componentesreceta);

            foreach (var componenteDTO in recetaEditada.Componentes)
            {
                var componente = await _baseDatos.Componentes.FirstOrDefaultAsync(c => c.Id == componenteDTO.Id);
                if (componente == null)
                {
                    return BadRequest(new AuthResponseDTO
                    {
                        IsSuccess = false,
                        Message = $"Componente con ID {componenteDTO.Id} no encontrado"
                    });
                }

                recetaExistente.Componentesreceta.Add(new Componentesrecetum
                {
                    ComponentesId = componenteDTO.Id,
                    Cantidad = componenteDTO.Cantidad,
                    Estatus = true,
                    RecetaId = recetaExistente.Id,
                    Componentes = componente
                });
            }

            await _baseDatos.SaveChangesAsync();

            return Ok(new AuthResponseDTO
            {
                IsSuccess = true,
                Message = "Receta actualizada exitosamente"
            });
        }

        [HttpPut("estatus-receta")]
        public async Task<IActionResult> EditarEstatusReceta([FromBody] RecetaEstatusDTO estatusDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Datos del modelo no válidos"
                });
            }

            var recetaExistente = await _baseDatos.Receta
                .Include(r => r.Componentesreceta)
                .FirstOrDefaultAsync(r => r.Id == estatusDTO.RecetaId);

            if (recetaExistente == null)
            {
                return NotFound(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Receta no encontrada"
                });
            }

            // Actualizar el estatus de la receta
            recetaExistente.Estatus = estatusDTO.EstatusReceta;

            // Actualizar el estatus de los componentes
            foreach (var componenteEstatus in estatusDTO.Componentes)
            {
                var componenteReceta = recetaExistente.Componentesreceta
                    .FirstOrDefault(cr => cr.ComponentesId == componenteEstatus.ComponenteId);

                if (componenteReceta != null)
                {
                    componenteReceta.Estatus = componenteEstatus.EstatusComponente;
                }
            }

            await _baseDatos.SaveChangesAsync();

            return Ok(new AuthResponseDTO
            {
                IsSuccess = true,
                Message = "Estatus de receta y componentes actualizados exitosamente"
            });
        }


    }
}
