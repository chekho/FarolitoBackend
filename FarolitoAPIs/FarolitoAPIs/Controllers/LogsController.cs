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
    public class LogsController : Controller
    {
        private readonly FarolitoDbContext _baseDatos;
        public LogsController(FarolitoDbContext baseDatos)
        {
            _baseDatos = baseDatos;
        }
        /* 
         * Recetas, proveedores, componentes
         */

        [Authorize(Roles = "Administrador")]
        [HttpGet("get-logs")]
        public async Task<IActionResult> GetLogs([FromQuery] string moduloNombre)
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

            var moduloExists = _baseDatos.Modulos.Where(m => m.Nombre == moduloNombre).FirstOrDefault();

            if (moduloExists == null)
            {
                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Módulo inexistente"
                });
            }
            else
            {
                if (moduloExists.Nombre != "Producción")
                {
                    var logs = await _baseDatos.Logs.Include(l => l.Modulo).Include(l => l.Usuario)
                        .Where(l => l.ModuloId == moduloExists.Id)
                        .Select(l => new
                        {
                            l.Id,
                            l.FechaHora,
                            l.Cambio,
                            usuario = l.Usuario.FullName
                        }).ToListAsync();

                    return Ok(logs);
                }
                else
                {
                    string[] statues = ["Rechazada", "Solicitado", "Autorizada", "Soldando", "Armando", "Calidad", "Terminado"];

                    var producciones = await _baseDatos.Produccions
                        .Include(p => p.Solicitudproduccion)
                        .ThenInclude(sp => sp.Usuario) 
                        .Select(p => new
                        {
                            p.Id,
                            FechaHora = p.Fecha,
                            Cambio = statues[(int)p.Solicitudproduccion.Estatus],
                            usuario = p.Usuario.FullName
                        }).ToListAsync();

                    return Ok(producciones);
                }
            }
        }

        [AllowAnonymous]
        [HttpGet("add-log")]
        public AuthResponseDTO AddLog(LogDTO logDTO)
        {
            var moduls = _baseDatos.Modulos.Where(m => m.Nombre == logDTO.Modulo).First();

            Logs newLog = new Logs
            {
                FechaHora = DateTime.Now,
                Cambio = logDTO.Cambio,
                ModuloId = moduls.Id,
                UsuarioId = logDTO.UsuarioId
            };

            try
            {
                _baseDatos.Logs.Add(newLog);
                _baseDatos.SaveChanges();

                return new AuthResponseDTO
                {
                    IsSuccess = true,
                    Message = "Log guardado"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = ex.StackTrace
                };
            }
        }
    }
}
