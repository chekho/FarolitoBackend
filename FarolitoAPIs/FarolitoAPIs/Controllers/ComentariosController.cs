using System.Security.Claims;
using FarolitoAPIs.Data;
using FarolitoAPIs.DTOs;
using FarolitoAPIs.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace FarolitoAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComentariosController : ControllerBase
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly FarolitoDbContext _baseDatos;
        public ComentariosController(FarolitoDbContext baseDatos, UserManager<Usuario> userManager)
        {
            _baseDatos = baseDatos;
            _userManager = userManager;
        }

        [HttpGet("comentarios")]
        public async Task<IActionResult> GetComentarios()
        {
            var listaComentarios = await _baseDatos.Comentarioos.ToListAsync();

            if (listaComentarios == null || !listaComentarios.Any())
            {
                Log.Warning("No comments found in the database");
                return NotFound(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "No se encontraron comentarios"
                });
                
            }
            return Ok(listaComentarios);
        }

        [HttpPost("agregar-comentario")]
        public async Task<IActionResult> AgregarComentario([FromBody] ComentarioDTO comentarioDTO)
        {
            if(comentarioDTO == null || string.IsNullOrWhiteSpace(comentarioDTO.Descripcion))
            {
                return BadRequest(new
                {
                    IsSuccess = false,
                    Message = "La descripcion del comentario no puede estar vacia."
                });
            }
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                Log.Warning("User not found for ID: {UserId}", userId);

                return NotFound(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "User not found"
                });
            }
            
            try
            {
                var comentario = new Comentarios
                {
                    Descripcion = comentarioDTO.Descripcion,
                    UserId = user.Id,
                    Fecha = DateTime.Now
                };
                await _baseDatos.Comentarioos.AddAsync(comentario);
                await _baseDatos.SaveChangesAsync();

                return CreatedAtAction(nameof(GetComentarios), new { id = comentario.Id }, new
                {
                    IsSuccess = true,
                    Message = "Comentario guardado exitosamente",
                    Data = new ComentarioDTO
                    {
                        Id = comentario.Id,
                        Descripcion = comentario.Descripcion,
                        UserId = user.Id,
                        Fecha = comentario.Fecha

                    }
                });
            }
            catch (Exception ex)
            {
                Log.Error("Error al crear un comentario: {Error}", ex.Message);
                return StatusCode(500, new
                {
                    IsSuccess = false,
                    Message = "Ocurrio un error al guardar el comentario."
                });
            }
        }

    }
}
