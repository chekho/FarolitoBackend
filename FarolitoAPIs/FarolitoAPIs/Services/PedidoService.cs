using System.Diagnostics;
using FarolitoAPIs.Data;
using FarolitoAPIs.Models;
using Microsoft.EntityFrameworkCore;

namespace FarolitoAPIs.Services;

public interface IPedidoService
{
    Task EnviarCorreoConfirmacionPedidoAsync(int ventaId, string usuarioId);
}

public class PedidoService : IPedidoService
{
    private readonly IEmailService _emailService;
    private readonly FarolitoDbContext _dbContext;
    private readonly ILogger<PedidoService> _logger;


    public PedidoService(IEmailService emailService, FarolitoDbContext dbContext, ILogger<PedidoService> logger)
    {
        _emailService = emailService;
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task EnviarCorreoConfirmacionPedidoAsync(int ventaId, string usuarioId)
    {
        var detallesVenta = await _dbContext.Detalleventa
            .Where(d => d.VentaId == ventaId)
            .Include(d => d.Inventariolampara).ThenInclude(inventariolampara => inventariolampara.Receta)
            .ToListAsync();

        var usuario = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == usuarioId);

        if (usuario == null)
        {
            throw new Exception("Usuario no encontrado");
        }

        var productos = string.Join("), ",
            detallesVenta.Select(d => $"{d.Inventariolampara.Receta.Nombrelampara} (Cantidad: {d.Cantidad}"));
        var total = detallesVenta.Sum(d => d.PrecioUnitario * d.Cantidad);

        var mensaje = $"Hola {usuario.FullName},\n\n" +
                      $"¡Gracias por tu compra en Farolito!\n\n" +
                      $"Tu pedido está en proceso. Aquí están los detalles de tu compra:\n" +
                      $"Número de pedido: {ventaId}\n" +
                      $"Productos comprados: {productos})\n" +
                      $"Total de la compra: ${total:0.00}\n\n" +
                      $"Nos pondremos en contacto contigo cuando tu pedido sea procesado. \n\n" +
                      $"¡Gracias por elegirnos!\n\n" +
                      $"saludos,\n" +
                      $"El equipo de Farolito";

        Debug.Assert(usuario.Email != null, "usuario.Email != null");
        await _emailService.EnviarCorreoAsync(usuario.Email, "Confirmación de tu compra en Farolito", mensaje);
        
        _logger.LogInformation($"Correo enviado a {usuario.Email} para el usuario {usuario.FullName}.");

        var historialComunicacion = new HistorialComunicacion
        {
            AccionRealizada = $"Se envió un correo de confirmación de compra para el pedido #{ventaId}",
            Fecha = DateTime.UtcNow,
            UsuarioId = usuarioId
        };
        _dbContext.HistorialComunicaciones.Add(historialComunicacion);
        
        _logger.LogInformation($"Registro en HistorialComunicaciones añadido para el usuario ID {usuario.Id}.");

        await _dbContext.SaveChangesAsync();
    }
}