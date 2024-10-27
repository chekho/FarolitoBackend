using FarolitoAPIs.Data;
using FarolitoAPIs.DTOs;
using FarolitoAPIs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Claims;

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
            //Log.Information("Fetching recipes from the database.");

            var recetas = await _baseDatos.Receta
                .Include(r => r.Componentesreceta)
                    .ThenInclude(cr => cr.Componentes)
                        .ThenInclude(c => c.Inventariocomponentes)
                            .ThenInclude(ic => ic.Detallecompra)
                .ToListAsync();

            if (recetas == null || !recetas.Any())
            {
                Log.Warning("No recipes found in the database.");

                return NotFound(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "No se encontraron recetas"
                });
            }

            //Log.Information("{RecipeCount} recipes found. Processing data.", recetas.Count);

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
                    Imagen = r.Imagen,
                    Componentes = componentesDTO
                    
                };
            }).ToList();

            //Log.Information("Successfully retrieved and processed {RecipeCount} recipes.", recetasDTO.Count);

            return Ok(recetasDTO);
        }

        [HttpGet("recetaspaginadas")]
        public async Task<IActionResult> ObtenerRecetaspag(int page = 1)
        {
            //Log.Information("Fetching paginated recipes. Requested page: {Page}", page);

            // Definir cuántas recetas quieres por página
            int pageSize = 1; // Cambia esto a la cantidad deseada por página

            var receta = await _baseDatos.Receta
                .Include(r => r.Componentesreceta)
                    .ThenInclude(cr => cr.Componentes)
                        .ThenInclude(c => c.Inventariocomponentes)
                            .ThenInclude(ic => ic.Detallecompra)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            if (receta == null || !receta.Any())
            {
                Log.Warning("No recipes found on page {Page}", page);

                return NotFound(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "No se encontró la receta"
                });
            }

            //Log.Information("{RecipeCount} recipes found on page {Page}. Processing data.", receta.Count, page);

            var recetasDTO = receta.Select(r => {
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
                    Imagen = r.Imagen,
                    Componentes = componentesDTO
                };
            }).ToList();

            Log.Information("Successfully retrieved and processed {RecipeCount} recipes from page {Page}.", recetasDTO.Count, page);

            return Ok(recetasDTO);
        }

        //[Authorize(Roles = "Administrador,Produccion")]
        [HttpPost("agregar-recetas")]
        public async Task<IActionResult> AgregarReceta([FromBody] RecetaDetalle2DTO nuevaReceta)
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

            //Log.Information("Attempting to add a new recipe: {RecipeName}", nuevaReceta.Nombrelampara);

            if (!ModelState.IsValid)
            {
                Log.Warning("Invalid model state for new recipe: {Errors}", ModelState.Values.SelectMany(v => v.Errors));

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
                Log.Warning("Some components were not found in the database. Expected: {Expected}, Found: {Found}",
                    nuevaReceta.Componentes.Count, componentes.Count);

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

            //Log.Information("Successfully added a new recipe: {RecipeName}", nuevaReceta.Nombrelampara);

            // LOG BD Agregar Receta
            LogDTO logDTO = new LogDTO
            {
                Cambio = "Agregó receta: " + receta.Nombrelampara,
                Modulo = "Recetas",
                UsuarioId = userId
            };

            LogsController lc = new LogsController(_baseDatos);
            AuthResponseDTO logVer = lc.AddLog(logDTO);

            if (logVer.IsSuccess)
            {
                return Ok(new AuthResponseDTO
                {
                    IsSuccess = true,
                    Message = "Receta agregada exitosamente"
                });
            }
            else
            {
                return BadRequest(logVer);
            }            
        }

        [HttpPut("actualizar-recetas")]
        public async Task<IActionResult> EditarReceta([FromBody] RecetaDetalle2DTO recetaEditada)
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

            //Log.Information("Attempting to edit recipe with ID: {RecipeId}", recetaEditada.Id);

            if (!ModelState.IsValid)
            {
                Log.Warning("Invalid model state for editing recipe: {Errors}", ModelState.Values.SelectMany(v => v.Errors));

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
                Log.Warning("Recipe with ID: {RecipeId} not found", recetaEditada.Id);

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
                    Log.Warning("Component with ID: {ComponentId} not found during recipe update", componenteDTO.Id);

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
            //Log.Information("Successfully updated recipe with ID: {RecipeId}", recetaEditada.Id);

            // LOG BD Actualización de receta
            LogDTO logDTO = new LogDTO
            {
                Cambio = "Actualización de receta " + recetaExistente.Nombrelampara,
                Modulo = "Recetas",
                UsuarioId = userId
            };

            LogsController lc = new LogsController(_baseDatos);
            AuthResponseDTO logVer = lc.AddLog(logDTO);

            if (logVer.IsSuccess)
            {
                return Ok(new AuthResponseDTO
                {
                    IsSuccess = true,
                    Message = "Receta actualizada exitosamente"
                });
            }
            else
            {
                return BadRequest(logVer);
            }
        }

        [HttpPut("estatus-receta")]
        public async Task<IActionResult> EditarEstatusReceta([FromBody] RecetaEstatusDTO estatusDTO)
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

            //Log.Information("Attempting to update status for recipe ID: {RecipeId}", estatusDTO.RecetaId);

            if (!ModelState.IsValid)
            {
                Log.Warning("Invalid model state for updating recipe status: {Errors}", ModelState.Values.SelectMany(v => v.Errors));

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
                Log.Warning("Recipe with ID: {RecipeId} not found", estatusDTO.RecetaId);

                return NotFound(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Receta no encontrada"
                });
            }

            recetaExistente.Estatus = estatusDTO.EstatusReceta;
            //Log.Information("Updated recipe status to: {EstatusReceta}", estatusDTO.EstatusReceta);

            foreach (var componenteEstatus in estatusDTO.Componentes)
            {
                var componenteReceta = recetaExistente.Componentesreceta
                    .FirstOrDefault(cr => cr.ComponentesId == componenteEstatus.ComponenteId);

                if (componenteReceta != null)
                {
                    componenteReceta.Estatus = componenteEstatus.EstatusComponente;
                    Log.Information("Updated component ID: {ComponentId} status to: {EstatusComponente}", componenteEstatus.ComponenteId, componenteEstatus.EstatusComponente);
                }
                else
                {
                    Log.Warning("Component ID: {ComponentId} not found in recipe components", componenteEstatus.ComponenteId);
                }
            }

            await _baseDatos.SaveChangesAsync();
            //Log.Information("Successfully updated recipe ID: {RecipeId} status and its components", estatusDTO.RecetaId);

            // LOG BD Actualización de receta
            LogDTO logDTO = new LogDTO
            {
                Cambio = "Actualización de receta " + recetaExistente.Nombrelampara,
                Modulo = "Recetas",
                UsuarioId = userId
            };

            LogsController lc = new LogsController(_baseDatos);
            AuthResponseDTO logVer = lc.AddLog(logDTO);

            if (logVer.IsSuccess)
            {
                return Ok(new AuthResponseDTO
                {
                    IsSuccess = true,
                    Message = "Estatus de receta y componentes actualizados exitosamente"
                });
            }
            else
            {
                return BadRequest(logVer);
            }
        }

        [HttpPut("recetasimagen")]
        public async Task<IActionResult> AgregarImagenReceta([FromForm] RecetaImagenDTO recetaImagen)
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

            //Log.Information("Attempting to update image for recipe ID: {RecipeId}", recetaImagen.Id);

            if (!ModelState.IsValid)
            {
                Log.Warning("Invalid model state for image upload: {Errors}", ModelState.Values.SelectMany(v => v.Errors));

                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Datos del modelo no válidos"
                });
            }

            // Buscar la receta por ID
            var receta = await _baseDatos.Receta.FindAsync(recetaImagen.Id);
            if (receta == null)
            {
                Log.Warning("Recipe with ID: {RecipeId} not found", recetaImagen.Id);

                return NotFound(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Receta no encontrada"
                });
            }

            // Verificar si la imagen no es nula y si es un archivo WebP
            if (recetaImagen.Imagen != null)
            {
                var extension = Path.GetExtension(recetaImagen.Imagen.FileName).ToLower();
                var mimeType = recetaImagen.Imagen.ContentType.ToLower();

                if (extension != ".webp" || mimeType != "image/webp")
                {
                    Log.Warning("Invalid image format for recipe ID: {RecipeId}. Expected .webp, received {MimeType}", recetaImagen.Id, mimeType);

                    return BadRequest(new AuthResponseDTO
                    {
                        IsSuccess = false,
                        Message = "Solo se permiten imágenes en formato WebP"
                    });
                }

                var fileName = $"{receta.Id}{extension}";
                var filePath = Path.Combine("wwwroot", "images", "recetas", fileName);

                // Crear el directorio si no existe
                var directoryPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryPath))
                {
                    //Log.Information("Creating directory: {DirectoryPath}", directoryPath);

                    Directory.CreateDirectory(directoryPath);
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    //Log.Information("Saving image to path: {FilePath}", filePath);

                    await recetaImagen.Imagen.CopyToAsync(stream);
                }

                // Actualizar la receta con la ruta de la imagen
                receta.Imagen = $"/images/recetas/{fileName}";
                _baseDatos.Receta.Update(receta);
                //Log.Information("Updated recipe ID: {RecipeId} with new image path: {ImagePath}", receta.Id, receta.Imagen);

            }

            await _baseDatos.SaveChangesAsync();
            Log.Information("Successfully updated image for recipe ID: {RecipeId}", recetaImagen.Id);

            // LOG BD Actualización imagen de receta
            LogDTO logDTO = new LogDTO
            {
                Cambio = "Actualización en imagen de receta " + receta.Nombrelampara,
                Modulo = "Recetas",
                UsuarioId = userId
            };

            LogsController lc = new LogsController(_baseDatos);
            AuthResponseDTO logVer = lc.AddLog(logDTO);

            if (logVer.IsSuccess)
            {
                return Ok(new AuthResponseDTO
                {
                    IsSuccess = true,
                    Message = "Imagen de receta actualizada exitosamente"
                });
            }
            else
            {
                return BadRequest(logVer);
            }
        }
    }
}
