using FarolitoAPIs.Data;
using FarolitoAPIs.DTOs;
using FarolitoAPIs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Serilog;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace FarolitoAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProduccionController : ControllerBase
    {

        /*
         * Agregar solicitud -> Crear solicitud /
         * Autorizar solicitud -> crear producción /
         * Rechazar solicitud -> estuatus = 0 /
         * terminar producción -> estatus = "terminado" 
         * 
         * Actualizar Producción -> soldado, armado, probado
         * 
         * 1 = Activo/ & producción = aceptado
         * 0 = rechazado
         * 2 & producción = Soldando
         * 3 & producción = Armando
         * 4 & producción = Terminado
        */

        private readonly FarolitoDbContext _baseDatos;

        public ProduccionController(FarolitoDbContext baseDatos)
        {
            _baseDatos = baseDatos;
        }

        [AllowAnonymous]
        [HttpGet("CargarSolicitudes")]
        public async Task<ActionResult<AuthResponseDTO>> CargarSolicitudes()
        {

            // Autenticar usuario
            var usuarioId = User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(usuarioId))
            {
                Log.Warning("CargarSolicitudes: User not authenticated. {UsuarioId}", usuarioId);

                return Unauthorized(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Usuario no autenticado"
                });
            }

            try
            {
                string[] statues = ["Rechazada", "Solicitado", "Autorizada", "Soldando", "Armando", "Calidad", "Terminado"];

                //Log.Information("CargarSolicitudes: Loading requests for user {UserId}", usuarioId);

                var solicitudesBD = await _baseDatos.Solicitudproduccions.Include(sp => sp.Receta).Include(sp => sp.Usuario).Include(sp => sp.Produccions).Where(sp => sp.Estatus == 1).Where(sp => !sp.Produccions.Any()).ToListAsync();

                var solicitudes = solicitudesBD.Select(s => new DetalleSolicitudProduccionDTO
                {
                    Id = s.Id,
                    Cantidad = s.Cantidad,
                    Descripcion = s.Descripcion,
                    Estatus = statues[s.Estatus ?? 0],
                    NombreUsuario = s.Usuario.FullName,
                    Receta = new DetalleRecetaDTO
                    {
                        Id = s.Receta.Id,
                        Nombrelampara = s.Receta.Nombrelampara
                    }
                });

                if (!solicitudes.Any())
                {
                    //Log.Information("CargarSolicitudes: No requests found for user {UserId}.", usuarioId);

                    return BadRequest(new AuthResponseDTO
                    {
                        IsSuccess = false,
                        Message = "sin información"
                    });
                }

                //Log.Information("CargarSolicitudes: Found {Count} requests for user {UserId}.", solicitudes.Count, usuarioId);
                return Ok(solicitudes);
            }
            catch (SqlException e)
            {
                Log.Error(e, "CargarSolicitudes: Database query error for user {UserId}.", usuarioId);
                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Algo salió mal..."
                });
            }
            catch (Exception e)
            {
                Log.Error(e, "CargarSolicitudes: Unexpected error while loading requests for user {UserId}.", usuarioId);
                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Algo salió mal..."
                });
            }

        }

        [AllowAnonymous]
        [HttpPost("AgregarSolicitud")]
        public async Task<IActionResult> AgregarSolicitud([FromBody] AgregarProduccionDTO nuevaSolicitud)
        {
            // Autenticar usuario
            var usuarioId = User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(usuarioId))
            {
                Log.Warning("AgregarSolicitud: User not authenticated. {UsuarioId}", usuarioId);

                return Unauthorized(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Usuario no autenticado"
                });
            }

            // instancias para buscar datos
            RecetaController recetaController = new RecetaController(_baseDatos);
            var actionResult = await recetaController.ObtenerRecetas() as ObjectResult;

            if (actionResult == null) {
                Log.Warning("AgregarSolicitud: No recipes found.");
                return NotFound(new AuthResponseDTO { IsSuccess = false, Message = "No se encontraron recetas" });
            } 

            var json = JsonConvert.SerializeObject(actionResult.Value);
            var list = JsonConvert.DeserializeObject<List<RecetaDetalleDTO>>(json);

            // Datos para la solicitud
            int idReceta = list.Where(obj => obj.Id == nuevaSolicitud.RecetaId).First().Id;

            Solicitudproduccion solicitudproduccion = new Solicitudproduccion
            {
                Descripcion = nuevaSolicitud.Descripcion,
                Cantidad = nuevaSolicitud.Cantidad,
                RecetaId = idReceta,
                UsuarioId = usuarioId,
                Estatus = 1
            };

            try
            {
                _baseDatos.Solicitudproduccions.Add(solicitudproduccion);
                _baseDatos.SaveChanges();
                //Log.Information("AgregarSolicitud: Request submitted successfully for user {UserId}.", usuarioId);

                return Ok(new AuthResponseDTO
                {
                    IsSuccess = true,
                    Message = "Solicitud enviada"
                });
            }
            catch (SqlException e)
            {
                Log.Error(e, "AgregarSolicitud: Database error while submitting request for user {UserId}.", usuarioId);
                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Algo salió mal..."
                });
            }
            catch (Exception e)
            {
                Log.Error(e, "AgregarSolicitud: Unexpected error while submitting request for user {UserId}.", usuarioId);
                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Algo salió mal..."
                });
            }
        }

        [AllowAnonymous]
        [HttpPost("AutorizarSolicitud")]
        public async Task<IActionResult> AutorizarSolicitud([FromBody] int idSolicitud)
        {

            // Autenticar usuario
            var usuarioId = User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(usuarioId))
            {
                Log.Warning("AutorizarSolicitud: User not authenticated. {UsuarioId}", usuarioId);
                return Unauthorized(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Usuario no autenticado"
                });
            }

            try
            {
                // instancias para buscar datos
                RecetaController recetaController = new RecetaController(_baseDatos);
                var actionResult = await recetaController.ObtenerRecetas() as ObjectResult;

                if (actionResult == null) {
                    Log.Warning("AutorizarSolicitud: No recipes found.");
                    return NotFound(new AuthResponseDTO { IsSuccess = false, Message = "No se encontraron recetas" });
                }

                var solicitudBD = _baseDatos.Solicitudproduccions.Include(sp => sp.Receta).Include(sp => sp.Usuario).Where(sp => sp.Id == idSolicitud).First();
                var json = JsonConvert.SerializeObject(actionResult.Value);
                var list = JsonConvert.DeserializeObject<List<RecetaDetalleDTO>>(json);

                if (solicitudBD != null)
                {
                    var recetaInfo = list!.Where(r => r.Id == solicitudBD.RecetaId).First();


                    solicitudBD.Estatus = 2;
                    Produccion produccion = new Produccion
                    {
                        Fecha = DateOnly.FromDateTime(DateTime.Now),
                        Costo = (double?)recetaInfo.CostoProduccion,
                        SolicitudproduccionId = idSolicitud,
                        UsuarioId = usuarioId
                    };

                    _baseDatos.Produccions.Add(produccion);
                    _baseDatos.SaveChanges();
                    //Log.Information("AutorizarSolicitud: Request {IdSolicitud} authorized successfully for user {UserId}.", idSolicitud, usuarioId);

                    return Ok(new AuthResponseDTO
                    {
                        IsSuccess = true,
                        Message = "Solicitud Autorizada con éxito"
                    });
                }
                else
                {
                    Log.Warning("AutorizarSolicitud: Request {IdSolicitud} not found.", idSolicitud);
                    return BadRequest(new AuthResponseDTO
                    {
                        IsSuccess = false,
                        Message = "Solicitud existente"
                    });
                }
            }
            catch (SqlException e)
            {
                Log.Error(e, "AutorizarSolicitud: Database error while authorizing request {IdSolicitud} for user {UserId}.", idSolicitud, usuarioId);

                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Algo salió mal..."
                });
            }
            catch (Exception e)
            {
                Log.Error(e, "AutorizarSolicitud: Unexpected error while authorizing request {IdSolicitud} for user {UserId}.", idSolicitud, usuarioId);

                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Algo salió mal..."
                });
            }
        }

        [AllowAnonymous]
        [HttpPatch("RechazarSolicitud")]
        public async Task<IActionResult> RechazarSolicitud([FromBody] SolicitudRechazoDTO solicitudRechazo)
        {
            // Autenticar usuario
            var usuarioId = User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(usuarioId))
            {
                Log.Warning("RechazarSolicitud: User not authenticated.");

                return Unauthorized(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Usuario no autenticado"
                });
            }

            if (!ModelState.IsValid)
            {
                Log.Warning("RechazarSolicitud: Invalid model state.");

                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Datos del modelo no válidos"
                });
            }

            var solicitudBD = _baseDatos.Solicitudproduccions.Where(sp => sp.Id == solicitudRechazo.Id).FirstOrDefault();

            if (solicitudBD == null) {
                Log.Warning("RechazarSolicitud: Request {Id} not found.", solicitudRechazo.Id);

                return BadRequest(new AuthResponseDTO { IsSuccess = false, Message = "Solicitud no encontrada" });
            } 
            else
            {
                var produccionBD = await _baseDatos.Produccions.Where(p => p.SolicitudproduccionId == solicitudRechazo.Id).ToListAsync();
                if (produccionBD.IsNullOrEmpty())
                {
                    solicitudBD.Descripcion = solicitudRechazo.Descripcion;
                    solicitudBD.Estatus = 0;

                    _baseDatos.SaveChanges();
                    //Log.Information("RechazarSolicitud: Request {Id} rejected successfully by user {UserId}.", solicitudRechazo.Id, usuarioId);

                    return Ok(new AuthResponseDTO
                    {
                        IsSuccess = false,
                        Message = "Solicitud Rechazada con éxito"
                    });
                }
                else
                {
                    Log.Warning("RechazarSolicitud: Request {Id} has already entered production.", solicitudRechazo.Id);
                    return BadRequest(new AuthResponseDTO
                    {
                        IsSuccess = false,
                        Message = "Ya entró a producción..."
                    });
                }
            }
        }

        [AllowAnonymous]
        [HttpGet("CargarProduciones")]
        public async Task<ActionResult<AuthResponseDTO>> CargarProducciones()
        {
            try
            {
                string[] statues = ["Rechazada", "Solicitado", "Autorizada", "Soldando", "Armando", "Calidad", "Terminado"];
                var produccionesBD = await _baseDatos.Produccions.Include(p => p.Usuario).Include(p => p.Solicitudproduccion).Include(p => p.Solicitudproduccion.Usuario).Include(p => p.Solicitudproduccion.Receta).Where(p => p.Solicitudproduccion.Estatus < 6).ToListAsync();

                var producciones = produccionesBD.Select(p => new DetallesProduccionDTO
                {
                    Id = p.Id,
                    Costo = p.Costo,
                    Fecha = p.Fecha,
                    NombreUsuario = p.Usuario.FullName,
                    SolicitudProduccion = new DetalleSolicitudProduccionDTO
                    {
                        Id = p.Solicitudproduccion.Id,
                        Cantidad = p.Solicitudproduccion.Cantidad,
                        Descripcion = p.Solicitudproduccion.Descripcion,
                        Estatus = statues[p.Solicitudproduccion.Estatus ?? 0],
                        NombreUsuario = p.Solicitudproduccion.Usuario.FullName,
                        Receta = new DetalleRecetaDTO
                        {
                            Id = p.Solicitudproduccion.Receta.Id,
                            Nombrelampara = p.Solicitudproduccion.Receta.Nombrelampara
                        }
                    }
                });

                if (producciones.Any()) {
                    //Log.Information("CargarProducciones: Successfully loaded productions.");

                    return Ok(producciones);
                } 
                else return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "No hay producciones..."
                });
            }
            catch (SqlException e)
            {
                Log.Error(e, "CargarProducciones: SQL error occurred.");

                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Algo salió mal..."
                });
            }
            catch (Exception e)
            {
                Log.Error(e, "CargarProducciones: An unexpected error occurred.");

                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Algo salió mal..." + e.ToString()
                });
            }

        }

        [AllowAnonymous]
        [HttpPatch("ActualizarProduccion")]
        public async Task<IActionResult> ActualizarProduccion([FromBody] int idProduccion)
        {
            // Autenticar usuario
            var usuarioId = User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(usuarioId))
            {
                Log.Warning("User not authenticated");
                return Unauthorized(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Usuario no autenticado"
                });
            }

            AuthResponseDTO respuesta = new AuthResponseDTO { IsSuccess = false, Message = "Algo salió mal" };

            string[] statues = ["Rechazada", "Solicitado", "Autorizada", "Soldando", "Armando", "Calidad", "Terminado"];
            var produccion = await _baseDatos.Produccions.Include(p => p.Solicitudproduccion).Include(sp => sp.Solicitudproduccion.Receta).Where(p => p.Id == idProduccion).FirstOrDefaultAsync();

            if (produccion != null)
            {

                if (produccion.Solicitudproduccion.Estatus == 0)
                {
                    respuesta.Message = "Producción no autorizada";
                    Log.Warning("ActualizarProduccion: {UserId} tried to update unauthorized production with ID {ProductionId}.", usuarioId, idProduccion);

                    return BadRequest(respuesta);
                }
                else if (produccion.Solicitudproduccion.Estatus == 6)
                {
                    respuesta.Message = "Producción ya terminada";
                    Log.Warning("ActualizarProduccion: {UserId} tried to update finished production with ID {ProductionId}.", usuarioId, idProduccion);

                    return BadRequest(respuesta);
                }
                else
                {
                    var solicitudBD = _baseDatos.Solicitudproduccions.Include(sp => sp.Receta).Include(sp => sp.Usuario).Where(sp => sp.Id == produccion.SolicitudproduccionId).First();
                    // 2 -> 3 & producción = Soldando | Descontar componentes
                    if (produccion.Solicitudproduccion.Estatus == 2)
                    {
                        List<Detalleproduccion> inventarioProduccion = new List<Detalleproduccion> { };

                        var ingredientesNecesarios = _baseDatos.Componentesreceta.Where(i => i.RecetaId == solicitudBD.RecetaId).ToList();

                        double? costoProduccion = 0;
                        double? cantidadProduccion = produccion.Solicitudproduccion.Cantidad;

                        if (ingredientesNecesarios.Any())
                        {
                            var inventarioDisponibleBD = _baseDatos.Inventariocomponentes
                                .Include(i => i.Detallecompra)
                                .Include(i => i.Detallecompra.Compra)
                                .Include(i => i.Componentes)
                                .Where(i => i.Cantidad > 0)
                                .OrderBy(i => i.Detallecompra.Compra.Fecha).ToList();
                            
                            var costoIngrediente = inventarioDisponibleBD.GroupBy(ic => ic.ComponentesId)
                                .Select(g => new { ComponentesId = g.Key, PromedioCostoPorCantidad = g.Average(ic => ic.Detallecompra.Costo) / g.Average(ic => ic.Detallecompra.Cantidad) })
                                .ToList();

                            ingredientesNecesarios.ForEach(ingrediente =>
                            {
                                Console.WriteLine(ingrediente.Componentes.Nombre);
                                //Log.Information("Processing ingredient: {IngredientName}", ingredient.Componentes.Nombre);

                                var inventarioDisponible = inventarioDisponibleBD.Where(i => i.ComponentesId == ingrediente.ComponentesId).ToList();

                                //Cal-culo de costo
                                costoProduccion += costoIngrediente.First(i => i.ComponentesId == ingrediente.ComponentesId).PromedioCostoPorCantidad;

                                if (inventarioDisponible.Any())
                                {
                                    int cantidadNecesaria = (int)(ingrediente.Cantidad * solicitudBD.Cantidad);
                                    bool comp = true;
                                    int i = 0;
                                    int cantidades = 0;

                                    cantidades = (int)(inventarioDisponible[i].Cantidad - cantidadNecesaria);

                                    Detalleproduccion detalleproduccion = new Detalleproduccion
                                    {
                                        InventariocomponentesId = inventarioDisponible[i].Id,
                                    };
                                    inventarioProduccion.Add(detalleproduccion);
                                    while (comp)
                                    {
                                        //DEscuento de materiales 
                                        inventarioDisponible[i].Cantidad = 0;
                                        i++;
                                        inventarioDisponible[i].Cantidad += cantidades;
                                        Detalleproduccion newDetalleproduccion = new Detalleproduccion
                                        {
                                            InventariocomponentesId = inventarioDisponible[i].Id,
                                        };
                                        inventarioProduccion.Add(newDetalleproduccion);
                                        if (inventarioDisponible[i].Cantidad >= cantidadNecesaria) { comp = false; }
                                    }
                                }
                            });
                        }

                        produccion.Detalleproduccions = inventarioProduccion;
                        produccion.Solicitudproduccion.Estatus = 3;
                        produccion.Costo = (int) costoProduccion;
                        //Log.Information("Production with ID {ProductionId} updated to Welding status.", production.Id);

                    }

                    // 3 -> 4 & producción = Armando
                    else if (produccion.Solicitudproduccion.Estatus == 3)
                    {
                        produccion.Solicitudproduccion.Estatus = 4;
                        //Log.Information("Production with ID {ProductionId} updated to Assembling status.", produccion.Id);

                    }
                    // 4 -> 5 & producción = Terminado | Agregar a inventario
                    else if (produccion.Solicitudproduccion.Estatus == 4)
                    {
                        string[] palabras = produccion!.Solicitudproduccion.Receta.Nombrelampara!.Split(' ');
                        produccion.Solicitudproduccion.Estatus = 5;
                        Inventariolampara inventariolampara = new Inventariolampara
                        {
                            Cantidad = produccion.Solicitudproduccion.Cantidad,
                            Lote = generarLoteLampara(palabras[palabras.Length - 1]),
                            FechaCreacion = DateOnly.FromDateTime(DateTime.Now),
                            Precio = produccion.Costo,
                            ProduccionId = produccion.Id,
                            RecetaId = produccion.Solicitudproduccion.RecetaId
                        };
                        _baseDatos.Inventariolamparas.Add(inventariolampara);
                        //Log.Information("Finished production added to inventory with ID {ProductionId}.", produccion.Id);

                    }
                    // 3 -> 4 & producción = Armando
                    else if (produccion.Solicitudproduccion.Estatus == 5)
                    {
                        respuesta.Message = "Siguiente paso es Terminado";
                        Log.Warning("ActualizarProduccion: {UserId} attempted to update already finished production with ID {ProductionId}.", usuarioId, idProduccion);
                        return BadRequest(respuesta);
                    }
                    else
                    {
                        respuesta.Message = "Estatus no válido";
                        Log.Warning("ActualizarProduccion: Invalid status for production with ID {ProductionId}.", idProduccion);
                        return BadRequest(respuesta);
                    }

                    try
                    {
                        _baseDatos.SaveChanges();

                        respuesta.IsSuccess = true;
                        if (produccion.Solicitudproduccion.Estatus == 5) respuesta.Message = "Producción en proceso de calidad";
                        else respuesta.Message = "Producción actualizada";

                        //Log.Information("Production with ID {ProductionId} updated successfully.", produccion.Id);

                        return Ok(respuesta);
                    }
                    catch (SqlException e)
                    {
                        Log.Error(e, "ActualizarProduccion: SQL error occurred while updating production with ID {ProductionId}.", produccion.Id);
                        return BadRequest(respuesta);
                    }
                    catch (Exception e)
                    {
                        Log.Error(e, "ActualizarProduccion: An unexpected error occurred while updating production with ID {ProductionId}.", produccion.Id);
                        return BadRequest(respuesta);
                    }
                }
            }
            else
            {
                respuesta.Message = "Solicitud de producción rechazada";
                Log.Warning("ActualizarProduccion: Production request with ID {ProductionId} rejected.", idProduccion);
                return BadRequest(respuesta);
            }
        }

        [AllowAnonymous]
        [HttpPatch("TerminarProduccion")]
        public async Task<IActionResult> TerminarProduccion([FromBody] int idProduccion)
        {

            //Log.Information("Iniciando la operación de terminar producción para el ID: {IdProduccion}", idProduccion);

            // Autenticar usuario
            var usuarioId = User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(usuarioId))
            {
                Log.Warning("User not authenticated when trying to end production.");

                return Unauthorized(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Usuario no autenticado"
                });
            }

            AuthResponseDTO respuesta = new AuthResponseDTO { IsSuccess = false, Message = "Algo salió mal" };

            var inventario = await _baseDatos.Inventariolamparas.Include(p => p.Produccion).Include(p => p.Produccion.Solicitudproduccion).Where(p => p.ProduccionId == idProduccion).FirstOrDefaultAsync();

            if (inventario != null)
            {
                if (inventario.Produccion.Solicitudproduccion.Estatus != 5)
                {
                    Log.Warning("Production not valid for termination. Current status: {Estatus}", inventario.Produccion.Solicitudproduccion.Estatus);
                    respuesta.Message = "Producción no válida para finalizar";
                    return BadRequest(respuesta);
                }
                else
                {
                    inventario.Produccion.Solicitudproduccion.Estatus = 6;
                    //Log.Information("Actualizando estatus de producción a terminado para el ID: {IdProduccion}", idProduccion);


                    try
                    {
                        _baseDatos.SaveChanges();

                        respuesta.Message = "Producción terminada";
                        respuesta.IsSuccess = true;
                        //Log.Information("Producción terminada exitosamente para el ID: {IdProduccion}", idProduccion);


                        return Ok(respuesta);
                    }
                    catch (SqlException e)
                    {
                        Log.Error(e, "Error saving changes to the database.");

                        return BadRequest(respuesta);
                    }
                    catch (Exception e)
                    {
                        Log.Error(e, "An unexpected error occurred.");
                        return BadRequest(respuesta);
                    }

                }
            }
            Log.Warning("Production already completed for ID: {IdProduccion}", idProduccion);

            respuesta.Message = "Producción ya terminada";
            return BadRequest(respuesta);
        }

        [AllowAnonymous]
        [HttpGet("GenerarLote")]
        public string generarLoteLampara(string nombreLampara)
        {
            Random _random = new Random();
            string nombreClean = Regex.Replace(nombreLampara, "[^a-zA-Z]", "");
            string lote = "L" + nombreLampara.Substring(0, 3).ToUpper() + "-" + DateOnly.FromDateTime(DateTime.Now).ToString("yyyyMMdd");

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            for (int i = 0; i < 3; i++) lote += chars[_random.Next(chars.Length)];

            return lote;
        }
    }
}
