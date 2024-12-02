using FarolitoAPIs.Data;
using FarolitoAPIs.DTOs;
using FarolitoAPIs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FarolitoAPIs.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HistorialComunicacionesController : ControllerBase
{
    private readonly FarolitoDbContext _context;

    public HistorialComunicacionesController(FarolitoDbContext context)
    {
        _context = context;
    }
    
    // Endpoint para recuperar todos los registros de HistorialComunicacion
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<HistorialComunicacion>>> GetHistorialComunicaciones()
    {
        var historialComunicacionesDto = await _context.HistorialComunicaciones
            .Include(h => h.usuario)
            .Select(h => new HistorialComunicacionDto
            {
                Id = h.Id,
                AccionRealizada = h.AccionRealizada,
                Fecha = h.Fecha,
                UsuarioId = h.UsuarioId,
            })
            .ToListAsync();
        
        return Ok(historialComunicacionesDto);
    }
    
    // Endpoint para recuperar los registros de HistorialComunicacion por UsuarioId
    [Authorize]
    [HttpGet("usuario/{usuarioId}")]
    public async Task<ActionResult<IEnumerable<HistorialComunicacion>>> GetHistorialComunicacionesByUsuario(
        string usuarioId)
    {
        var historialComunicacionesDto = await _context.HistorialComunicaciones
            .Where(h => h.UsuarioId == usuarioId)
            .Include(h => h.usuario)
            .Select(h => new HistorialComunicacionDto
            {
                Id = h.Id,
                AccionRealizada = h.AccionRealizada,
                Fecha = h.Fecha,
                UsuarioId = h.UsuarioId,
            })
            .ToListAsync();

        if (!historialComunicacionesDto.Any())
        {
            return NotFound(new AuthResponseDTO
            {
                IsSuccess = true,
                Message = $"No se encontraron registros de comunicación para el usuario con ID: {usuarioId}"

            });
        }
        
        return Ok(historialComunicacionesDto);
    }
}
