using FarolitoAPIs.Data;
using FarolitoAPIs.DTOs;
using FarolitoAPIs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Serilog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FarolitoAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MermasController : ControllerBase
    {
        private readonly FarolitoDbContext _baseDatos;
        public MermasController(FarolitoDbContext baseDatos)
        {
            _baseDatos = baseDatos;
        }

        [HttpGet("mermasComponentes")]
        public async Task<ActionResult<AuthResponseDTO>> GetMermasComponentes()
        {
            //Log.Information("Request received to fetch component waste details.");

            var mermasBD = await _baseDatos.Mermacomponentes.Include(m => m.Usuario).Include(m => m.Inventariocomponentes).Include(m => m.Inventariocomponentes.Componentes).ToListAsync();
            var mermas = mermasBD.Select(m => new DetalleMermaComponenteDTO{
                Id = m.Id,
                Cantidad = m.Cantidad,
                Descripcion = m.Descripcion,
                Fecha = m.Fecha,
                Usuario = m.Usuario.FullName,
                Componente = m.Inventariocomponentes.Componentes.Nombre
            });

            if (!mermas.Any()) {
                Log.Warning("No component waste information found.");
                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "sin información"
                });
            }

            //Log.Information("Successfully retrieved {Count} component waste details.", mermas.Count);

            return Ok(mermas);
        }
        
        [HttpGet("mermasComponentesUsuario")]
        public async Task<ActionResult<AuthResponseDTO>> GetMermasComponentesUsuario()
        {
            //Log.Information("Request received to fetch component waste details for the authenticated user.");


            // Autenticar usuario
            var usuarioId = User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(usuarioId))
            {
                Log.Warning("Unauthorized access attempt. User ID is null or empty.");

                return Unauthorized(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Usuario no autenticado"
                });
            }

            var mermasBD = await _baseDatos.Mermacomponentes.Include(m => m.Usuario).Include(m => m.Inventariocomponentes).Include(m => m.Inventariocomponentes.Componentes).Where(m => m.UsuarioId == usuarioId).ToListAsync();
            var mermas = mermasBD.Select(m => new DetalleMermaComponenteDTO{
                Id = m.Id,
                Cantidad = m.Cantidad,
                Descripcion = m.Descripcion,
                Fecha = m.Fecha,
                Usuario = m.Usuario.FullName,
                Componente = m.Inventariocomponentes.Componentes.Nombre
            });
            if (!mermas.Any()) {
                //Log.Information("No component waste information found for user ID {UserId}.", usuarioId);
                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "sin información"
                });
            }
            //Log.Information("Successfully retrieved {Count} component waste details for user ID {UserId}.", mermas.Count, usuarioId);

            return Ok(mermas);
        }

        [HttpPost("mermaComponente")]
        public async Task<ActionResult<AuthResponseDTO>> MermarComponente([FromBody] MermaComponenteDTO merma)
        {
            //Log.Information("Request received to mark component as waste. User ID: {UserId}", User?.FindFirstValue(ClaimTypes.NameIdentifier));


            // Autenticar usuario
            var usuarioId = User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(usuarioId))
            {
                Log.Warning("Unauthorized access attempt. User ID is null or empty.");

                return Unauthorized(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Usuario no autenticado"
                });
            }


            try
            {
                var inventario = await _baseDatos.Inventariocomponentes.Where(i => i.Id == merma.InventarioComponenteId).FirstOrDefaultAsync();
                if (inventario != null)
                {
                    if(merma.Cantidad > inventario.Cantidad)
                    {
                        Log.Warning("Attempt to mark waste with quantity exceeding available stock. Requested: {RequestedQuantity}, Available: {AvailableQuantity}", merma.Cantidad, inventario.Cantidad);

                        return BadRequest(new AuthResponseDTO
                        {
                            IsSuccess = false,
                            Message = "Cantidad no permitida"
                        });
                    }

                    Mermacomponente mermacomponente = new Mermacomponente
                    {
                        Cantidad = merma.Cantidad,
                        Descripcion = merma.Descripcion,
                        Fecha = DateOnly.FromDateTime(DateTime.Now),
                        InventariocomponentesId = merma.InventarioComponenteId,
                        UsuarioId = usuarioId
                    };

                    inventario.Cantidad -= merma.Cantidad;

                    _baseDatos.Mermacomponentes.Add(mermacomponente);

                    await _baseDatos.SaveChangesAsync();
                    //Log.Information("Component marked as waste successfully. Component ID: {ComponentId}, Quantity: {Quantity}", merma.InventarioComponenteId, merma.Cantidad);

                    return Ok(new AuthResponseDTO
                    {
                        IsSuccess = true,
                        Message = "Componente mermado con éxito"
                    });
                }
                else
                {
                    Log.Warning("Component not found. Component ID: {ComponentId}", merma.InventarioComponenteId);

                    return BadRequest(new AuthResponseDTO
                    {
                        IsSuccess = false,
                        Message = "Componente no encontrado..."
                    });
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "An error occurred while marking component as waste.");

                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Algo salió mal"
                });
            }
        }
        
        //***************************************************************
        
        [HttpGet("mermasLamparas")]
        public async Task<ActionResult<AuthResponseDTO>> GetMermasLamparas()
        {
            Log.Information("Request received to retrieve waste information for lamps.");


            var mermasBD = await _baseDatos.Mermalamparas.Include(m => m.Usuario).Include(m => m.Inventariolampara).Include(m => m.Inventariolampara.Receta).ToListAsync();
            var mermas = mermasBD.Select(m => new DetalleMermaLamparaDTO
            {
                Id = m.Id,
                Cantidad = m.Cantidad,
                Descripcion = m.Descripcion,
                Fecha = m.Fecha,
                Usuario = m.Usuario.FullName,
                Lampara = m.Inventariolampara.Receta.Nombrelampara
            });
            if (!mermas.Any()) {
                Log.Warning("No waste information found for lamps.");

                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "sin información"
                });
            }
            //Log.Information("Successfully retrieved waste information for lamps. Count: {Count}", mermas.Count);

            return Ok(mermas);
        }
        
        [HttpGet("mermasLamparasUsuario")]
        public async Task<ActionResult<AuthResponseDTO>> GetMermasLamparasUsuario()
        {
            //Log.Information("Request received to retrieve lamp waste information for the authenticated user.");


            // Autenticar usuario
            var usuarioId = User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(usuarioId))
            {
                Log.Warning("Unauthorized access attempt: User not authenticated.");

                return Unauthorized(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Usuario no autenticado"
                });
            }

            var mermasBD = await _baseDatos.Mermalamparas.Include(m => m.Usuario).Include(m => m.Inventariolampara).Include(m => m.Inventariolampara.Receta).Where(m => m.UsuarioId == usuarioId).ToListAsync();
            
            var mermas = mermasBD.Select(m => new DetalleMermaLamparaDTO
            {
                Id = m.Id,
                Cantidad = m.Cantidad,
                Descripcion = m.Descripcion,
                Fecha = m.Fecha,
                Usuario = m.Usuario.FullName,
                Lampara = m.Inventariolampara.Receta.Nombrelampara
            });
            if (!mermas.Any()) {
                //Log.Information("No waste information found for user with ID: {UserId}.", usuarioId);

                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "sin información"
                });
            }
            //Log.Information("Successfully retrieved waste information for user with ID: {UserId}. Count: {Count}", usuarioId, mermas.Count);

            return Ok(mermas);
        }

        [HttpPost("mermaLampara")]
        public async Task<ActionResult<AuthResponseDTO>> MermarLampara([FromBody] MermaLamparaDTO merma)
        {

            //Log.Information("Request received to reduce lamp inventory.");

            // Autenticar usuario
            var usuarioId = User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(usuarioId))
            {
                Log.Warning("Unauthorized access attempt: User not authenticated.");

                return Unauthorized(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Usuario no autenticado"
                });
            }


            try
            {
                var lampara = await _baseDatos.Inventariolamparas.Where(l => l.Id == merma.InventariolamparaId).FirstOrDefaultAsync();
                if (lampara != null)
                {
                    if (merma.Cantidad > lampara.Cantidad)
                    {
                        Log.Warning("Attempted to reduce lamp quantity by {Cantidad}, but only {Available} available.", merma.Cantidad, lampara.Cantidad);

                        return BadRequest(new AuthResponseDTO
                        {
                            IsSuccess = false,
                            Message = "Cantidad no permitida"
                        });
                    }
                    Mermalampara mermaLampara = new Mermalampara
                    {
                        Cantidad = merma.Cantidad,
                        Descripcion = merma.Descripcion,
                        Fecha = DateOnly.FromDateTime(DateTime.Now),
                        InventariolamparaId = merma.InventariolamparaId,
                        UsuarioId = usuarioId
                    };

                    lampara.Cantidad -= merma.Cantidad;

                    _baseDatos.Mermalamparas.Add(mermaLampara);
                    await _baseDatos.SaveChangesAsync();
                    //Log.Information("Lamp with ID {LampId} successfully reduced by {Cantidad} by user {UserId}.", merma.InventariolamparaId, merma.Cantidad, usuarioId);

                    return Ok(new AuthResponseDTO
                    {
                        IsSuccess = true,
                        Message = "Lámpara mermada con éxito"
                    });
                }
                else
                {
                    Log.Warning("Lamp with ID {LampId} not found.", merma.InventariolamparaId);

                    return BadRequest(new AuthResponseDTO
                    {
                        IsSuccess = false,
                        Message = "Lámpara no encontrada..."
                    });
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "An error occurred while reducing lamp inventory for user ID: {UserId}.", usuarioId);

                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Algo salió mal..."
                });
            }
        }
    }
}
