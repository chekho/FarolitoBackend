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
            try{
                var solicitudesBD = await _baseDatos.Solicitudproduccions.Include(sp => sp.Receta).Include(sp => sp.Usuario).Include(sp => sp.Produccions).Where(sp => sp.Estatus==1).Where(p => !p.Produccions.Any()).ToListAsync();
                
                var solicitudes = solicitudesBD.Select(s => new DetalleSolicitudProduccionDTO
                {
                    Id = s.Id,
                    Cantidad = s.Cantidad,
                    Descripcion = s.Descripcion,
                    Estatus = s.Estatus,
                    NombreUsuario = s.Usuario.FullName,
                    Receta = new DetalleRecetaDTO
                    {
                        Id = s.Receta.Id,
                        Nombrelampara = s.Receta.Nombrelampara
                    }
                });

                return Ok(solicitudes);
            } catch (SqlException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
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
            
            if (actionResult == null) return NotFound(new AuthResponseDTO { IsSuccess = false, Message = "No se encontraron recetas"});
            
            var json = JsonConvert.SerializeObject(actionResult.Value);
            var list = JsonConvert.DeserializeObject<List<RecetaDetalleDTO>>(json);
            
            // Datos para la solicitud
            int idReceta = list.Where(obj => obj.Id == nuevaSolicitud.RecetaId).First().Id;

            Solicitudproduccion solicitudproduccion = new Solicitudproduccion { 
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
                    Message = "Solicitud enviada..."
                });
            } catch (SqlException e)
            {
                return BadRequest(e.Message);
            } catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("AutorizarSolicitud")]
        public async Task<IActionResult> AutorizarSolicitud([FromBody] int idSolicitud)
        {
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

                    //var verProd = _baseDatos.Produccions.Where(p => p.SolicitudproduccionId == idSolicitud).First();
                
                    List<Detalleproduccion> inventarioProduccion = new List<Detalleproduccion> { };

                    var ingredientesNecesarios = _baseDatos.Componentesreceta.Where(i => i.RecetaId == solicitudBD.RecetaId).ToList();

                    if (ingredientesNecesarios.Any())
                    {
                        var inventarioDisponibleBD = _baseDatos.Inventariocomponentes
                            .Include(i => i.Detallecompra)
                            .Include(i => i.Detallecompra.Compra)
                            .Where(i => i.Cantidad > 0)
                            .OrderBy(i => i.Detallecompra.Compra.Fecha).ToList();

                        ingredientesNecesarios.ForEach(ingrediente =>
                        {
                            var inventarioDisponible = inventarioDisponibleBD.Where(i => i.ComponentesId == ingrediente.ComponentesId).ToList();

                            if (inventarioDisponible.Any())
                            {
                                int cantidadNecesaria = (int)(ingrediente.Cantidad * solicitudBD.Cantidad);

                                bool comp = false;
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
                                    inventarioDisponible[i].Cantidad = 0;
                                    i++;
                                    inventarioDisponible[i].Cantidad += cantidades;
                                    Detalleproduccion newDetalleproduccion = new Detalleproduccion
                                    {
                                        InventariocomponentesId = inventarioDisponible[i].Id,
                                    };
                                    inventarioProduccion.Add(newDetalleproduccion);
                                    if (inventarioDisponible[i].Cantidad >= cantidadNecesaria) { comp = true; }
                                }
                            }
                        });

                    }

                    Produccion produccion = new Produccion
                    {
                        Fecha = DateOnly.FromDateTime(DateTime.Now),
                        Costo = (double?)recetaInfo.CostoProduccion,
                        SolicitudproduccionId = idSolicitud,
                        UsuarioId = "5",
                        Detalleproduccions = inventarioProduccion
                    };

                    _baseDatos.Produccions.Add(produccion);
                    _baseDatos.SaveChanges();
                    return Ok("Solicitud enviada...");
                }
                else
                {
                    return BadRequest("Solicitud existente");
                }
            }
            catch (SqlException e)
            {
                return BadRequest("SqlException " + e.Message);
            }
            catch (Exception e)
            {
                return BadRequest("Eception "+e.Message);
            }
        }

        [AllowAnonymous]
        [HttpPatch("RechazarSolicitud")]
        public async Task<IActionResult> RechazarSolicitud([FromBody] SolicitudRechazoDTO solicitudRechazo )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Datos del modelo no válidos"
                });
            }

            var solicitudBD = _baseDatos.Solicitudproduccions.Where(sp => sp.Id == solicitudRechazo.Id).FirstOrDefault();
            
            if (solicitudBD == null) return BadRequest("Solicitud no encontrada");
            else
            {
                var produccionBD = await _baseDatos.Produccions.Where(p => p.SolicitudproduccionId == solicitudRechazo.Id).ToListAsync();
                if(produccionBD.IsNullOrEmpty()){
                    solicitudBD.Descripcion = solicitudRechazo.Descripcion;
                    solicitudBD.Estatus = 0;

                    _baseDatos.SaveChanges();
                    return Ok("Solicitud Rechazada");
                } 
                else
                {
                    return BadRequest("Ya entró a producción");
                }
            }
        }

        [AllowAnonymous]
        [HttpGet("CargarProduciones")]
        public async Task<ActionResult<AuthResponseDTO>> CargarProducciones()
        {
            try
            {
                var produccionesBD = await _baseDatos.Produccions.Include(p => p.Usuario).Include(p => p.Solicitudproduccion).Include(p => p.Solicitudproduccion.Usuario).Include(p => p.Solicitudproduccion.Receta).Include(p=>p.Inventariolamparas).Where(p => p.Solicitudproduccion.Estatus != 0).Where(p => !p.Inventariolamparas.Any()).ToListAsync();
                
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
                        Estatus = p.Solicitudproduccion.Estatus,
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
                    Message = "Algo salió mal (info)"
                });
            }
            catch (SqlException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [AllowAnonymous]
        [HttpPatch("TerminarProduccion")]
        public async Task<IActionResult> TerminarProduccion([FromBody] int idProduccion )
        {
            var inventarioOld = await _baseDatos.Inventariolamparas.Where(p => p.ProduccionId == idProduccion).FirstOrDefaultAsync();
            
            if(inventarioOld == null){
                var produccion = await _baseDatos.Produccions.Include(p => p.Solicitudproduccion).Include(sp => sp.Solicitudproduccion.Receta).Where(p => p.Id == idProduccion).FirstOrDefaultAsync();

                if(produccion != null){
                    string[] palabras = produccion!.Solicitudproduccion.Receta.Nombrelampara!.Split(' ');
                
                    Inventariolampara inventariolampara = new Inventariolampara
                    {
                        Cantidad = produccion.Solicitudproduccion.Cantidad,
                        Lote = generarLoteLampara(palabras[palabras.Length - 1]),
                        FechaCreacion = DateOnly.FromDateTime(DateTime.Now),
                        Precio = produccion.Costo,
                        ProduccionId = produccion.Id,
                        RecetaId = produccion.Solicitudproduccion.RecetaId
                    };
                    Console.WriteLine(inventariolampara.Lote);
                    try
                    {
                        _baseDatos.Inventariolamparas.Add(inventariolampara);
                        _baseDatos.SaveChanges();
                        return Ok(inventariolampara);
                    }
                    catch (SqlException e)
                    {
                        return BadRequest(e);
                    }
                    catch (Exception e)
                    {
                        return BadRequest(e);
                    }
                }
                else{ return BadRequest("Produccion no encontrada"); }
            }
            else
            {
                return BadRequest("Produccion ya terminada");
            }
        }

        [AllowAnonymous]
        [HttpGet("GenerarLote")]
        public string generarLoteLampara(string nombreLampara)
        {
            Random _random = new Random();
            string nombreClean = Regex.Replace(nombreLampara, "[^a-zA-Z]", "");
            string lote = "L"+nombreLampara.Substring(0, 3).ToUpper()+"-"+ DateOnly.FromDateTime(DateTime.Now).ToString("yyyyMMdd");

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            for (int i = 0; i < 3; i++) lote += chars[_random.Next(chars.Length)];

            return lote;
        }
        /*
         * Agregar solicitud -> Crear solicitud /
         * Autorizar solicitud -> crear producción /
         * Rechazar solicitud -> estuatus = 0 /
         * terminar producción -> estatus = "terminado" 
        */
    }
}
