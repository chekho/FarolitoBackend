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
                .ToListAsync();

            if (recetas == null || !recetas.Any())
            {
                return NotFound(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "No se encontraron recetas"
                });
            }

            var recetasDTO = recetas.Select(r => new RecetaDetalleDTO
            {
                Id = r.Id,
                Nombrelampara = r.Nombrelampara,
                Estatus = r.Estatus,
                Componentes = r.Componentesreceta.Select(cr => new ComponenteRecetaDTO
                {
                    Id = cr.Componentes.Id,
                    Nombre = cr.Componentes.Nombre,
                    Cantidad = cr.Cantidad 
                }).ToList()
            }).ToList();

            return Ok(recetasDTO); 
        }

        //[Authorize(Roles = "Administrador,Produccion")]
        [HttpPost("recetas")]
        public async Task<IActionResult> AgregarReceta([FromBody] RecetaDetalleDTO nuevaReceta)
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


    }
}
