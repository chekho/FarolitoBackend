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

namespace FarolitoAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProduccionController : ControllerBase
    {
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
                return Unauthorized(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Usuario no autenticado"
                });
            }

            try
            {
                string[] statues = ["Rechazada", "Solicitado", "Autorizada", "Soldando", "Armando", "Calidad", "Terminado"];
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

                return Ok(solicitudes);
            }
            catch (SqlException e)
            {
                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Algo salió mal..."
                });
            }
            catch (Exception e)
            {
                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Algo salió mal..." + e.StackTrace
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
                return Unauthorized(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Usuario no autenticado"
                });
            }

            // instancias para buscar datos
            RecetaController recetaController = new RecetaController(_baseDatos);
            var actionResult = await recetaController.ObtenerRecetas() as ObjectResult;

            if (actionResult == null) return NotFound(new AuthResponseDTO { IsSuccess = false, Message = "No se encontraron recetas" });

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
                return Ok(new AuthResponseDTO
                {
                    IsSuccess = true,
                    Message = "Solicitud enviada"
                });
            }
            catch (SqlException e)
            {
                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Algo salió mal..."
                });
            }
            catch (Exception e)
            {
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

                if (actionResult == null) return NotFound(new AuthResponseDTO { IsSuccess = false, Message = "No se encontraron recetas" });

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
                    return Ok(new AuthResponseDTO
                    {
                        IsSuccess = true,
                        Message = "Solicitud Autorizada"
                    });
                }
                else
                {
                    return BadRequest(new AuthResponseDTO
                    {
                        IsSuccess = false,
                        Message = "Solicitud existente"
                    });
                }
            }
            catch (SqlException e)
            {
                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Algo salió mal..."
                });
            }
            catch (Exception e)
            {
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
                return Unauthorized(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Usuario no autenticado"
                });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Datos del modelo no válidos"
                });
            }

            var solicitudBD = _baseDatos.Solicitudproduccions.Where(sp => sp.Id == solicitudRechazo.Id).FirstOrDefault();

            if (solicitudBD == null) return BadRequest(new AuthResponseDTO { IsSuccess = false, Message = "Solicitud no encontrada" });
            else
            {
                var produccionBD = await _baseDatos.Produccions.Where(p => p.SolicitudproduccionId == solicitudRechazo.Id).ToListAsync();
                if (produccionBD.IsNullOrEmpty())
                {
                    solicitudBD.Descripcion = solicitudRechazo.Descripcion;
                    solicitudBD.Estatus = 0;

                    _baseDatos.SaveChanges();
                    return Ok(new AuthResponseDTO
                    {
                        IsSuccess = false,
                        Message = "Solicitud Rechazada"
                    });
                }
                else
                {
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

                if (producciones.Any()) return Ok(producciones);
                else return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "No hay producciones..."
                });
            }
            catch (SqlException e)
            {
                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Algo salió mal..." + e.ToString()
                });
            }
            catch (Exception e)
            {
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
                    return BadRequest(respuesta);
                }
                else if (produccion.Solicitudproduccion.Estatus == 6)
                {
                    respuesta.Message = "Producción ya terminada";
                    return BadRequest(respuesta);
                }
                else
                {
                    //List<> inventarios = [];
                    // ["Rechazada", "Solicitado", "Autorizada", "Soldando", "Armando", "Calidad", "Terminado"];
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
                        Console.WriteLine(costoProduccion);
                        produccion.Costo = (int) costoProduccion;
                    }
                    // 3 -> 4 & producción = Armando
                    else if (produccion.Solicitudproduccion.Estatus == 3)
                    {
                        produccion.Solicitudproduccion.Estatus = 4;
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
                    }
                    // 3 -> 4 & producción = Armando
                    else if (produccion.Solicitudproduccion.Estatus == 5)
                    {
                        respuesta.Message = "Siguiente paso es Terminado";
                        return BadRequest(respuesta);
                    }
                    else
                    {
                        return BadRequest(respuesta);
                    }

                    try
                    {
                        _baseDatos.SaveChanges();

                        respuesta.IsSuccess = true;
                        if (produccion.Solicitudproduccion.Estatus == 5) respuesta.Message = "Producción en proceso de calidad";
                        else respuesta.Message = "Producción actualizada";

                        return Ok(respuesta);
                    }
                    catch (SqlException e)
                    {
                        return BadRequest(respuesta);
                    }
                    catch (Exception e)
                    {
                        return BadRequest(respuesta);
                    }
                }
            }
            else
            {
                return BadRequest(respuesta);
            }
        }

        [AllowAnonymous]
        [HttpPatch("TerminarProduccion")]
        public async Task<IActionResult> TerminarProduccion([FromBody] int idProduccion)
        {
            // Autenticar usuario
            var usuarioId = User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(usuarioId))
            {
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
                    respuesta.Message = "Producción no válida para finalizar";
                    return BadRequest(respuesta);
                }
                else
                {
                    inventario.Produccion.Solicitudproduccion.Estatus = 6;

                    try
                    {
                        _baseDatos.SaveChanges();

                        respuesta.Message = "Producción terminada";
                        respuesta.IsSuccess = true;

                        return Ok(respuesta);
                    }
                    catch (SqlException e)
                    {
                        return BadRequest(respuesta);
                    }
                    catch (Exception e)
                    {
                        return BadRequest(respuesta);
                    }

                }
            }
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
    }
}
