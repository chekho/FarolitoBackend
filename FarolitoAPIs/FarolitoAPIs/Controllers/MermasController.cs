using FarolitoAPIs.Data;
using FarolitoAPIs.DTOs;
using FarolitoAPIs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
            var mermasBD = await _baseDatos.Mermacomponentes.Include(m => m.Usuario).Include(m => m.Inventariocomponentes).Include(m => m.Inventariocomponentes.Componentes).ToListAsync();
            var mermas = mermasBD.Select(m => new DetalleMermaComponenteDTO{
                Id = m.Id,
                Cantidad = m.Cantidad,
                Descripcion = m.Descripcion,
                Fecha = m.Fecha,
                Usuario = m.Usuario.FullName,
                Componente = m.Inventariocomponentes.Componentes.Nombre
            });
            if (!mermas.Any()) return BadRequest(new AuthResponseDTO
            {
                IsSuccess = false,
                Message = "sin información"
            });
            return Ok(mermas);
        }
        
        [HttpGet("mermasComponentesUsuario")]
        public async Task<ActionResult<AuthResponseDTO>> GetMermasComponentesUsuario()
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

            var mermasBD = await _baseDatos.Mermacomponentes.Include(m => m.Usuario).Include(m => m.Inventariocomponentes).Include(m => m.Inventariocomponentes.Componentes).Where(m => m.UsuarioId == usuarioId).ToListAsync();
            var mermas = mermasBD.Select(m => new DetalleMermaComponenteDTO{
                Id = m.Id,
                Cantidad = m.Cantidad,
                Descripcion = m.Descripcion,
                Fecha = m.Fecha,
                Usuario = m.Usuario.FullName,
                Componente = m.Inventariocomponentes.Componentes.Nombre
            });
            if (!mermas.Any()) return BadRequest(new AuthResponseDTO
            {
                IsSuccess = false,
                Message = "sin información"
            });
            return Ok(mermas);
        }

        [HttpPost("mermaComponente")]
        public async Task<ActionResult<AuthResponseDTO>> MermarComponente([FromBody] MermaComponenteDTO merma)
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
                var inventario = await _baseDatos.Inventariocomponentes.Where(i => i.Id == merma.InventarioComponenteId).FirstOrDefaultAsync();
                if (inventario != null)
                {
                    if(merma.Cantidad > inventario.Cantidad)
                    {
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
                    return Ok(new AuthResponseDTO
                    {
                        IsSuccess = true,
                        Message = "Componente mermado con éxito"
                    });
                }
                else
                {
                    return BadRequest(new AuthResponseDTO
                    {
                        IsSuccess = false,
                        Message = "Componente no encontrado..."
                    });
                }
            }
            catch (Exception e)
            {
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
            if (!mermas.Any()) return BadRequest(new AuthResponseDTO
            {
                IsSuccess = false,
                Message = "sin información"
            });
            return Ok(mermas);
        }
        
        [HttpGet("mermasLamparasUsuario")]
        public async Task<ActionResult<AuthResponseDTO>> GetMermasLamparasUsuario()
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
            if (!mermas.Any()) return BadRequest(new AuthResponseDTO
            {
                IsSuccess = false,
                Message = "sin información"
            });
            return Ok(mermas);
        }

        [HttpPost("mermaLampara")]
        public async Task<ActionResult<AuthResponseDTO>> MermarLampara([FromBody] MermaLamparaDTO merma)
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
                var lampara = await _baseDatos.Inventariolamparas.Where(l => l.Id == merma.InventariolamparaId).FirstOrDefaultAsync();
                if (lampara != null)
                {
                    if (merma.Cantidad > lampara.Cantidad)
                    {
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
                    return Ok(new AuthResponseDTO
                    {
                        IsSuccess = true,
                        Message = "Lámpara mermada con éxito"
                    });
                }
                else
                {
                    return BadRequest(new AuthResponseDTO
                    {
                        IsSuccess = false,
                        Message = "Lámpara no encontrada..."
                    });
                }
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
    }
}
