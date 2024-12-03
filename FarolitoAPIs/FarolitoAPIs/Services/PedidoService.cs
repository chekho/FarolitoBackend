using FarolitoAPIs.Data;
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

    public PedidoService(IEmailService emailService, FarolitoDbContext dbContext)
    {
        _emailService = emailService;
        _dbContext = dbContext;
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

        var productos = string.Join(", ",
            detallesVenta.Select(d => $"{d.Inventariolampara.Receta.Nombrelampara} (Cantidad: {d.Cantidad}"));
        var total = detallesVenta.Sum(d => d.PrecioUnitario * d.Cantidad);

        var mensaje = $"Hola {usuario.FullName},\n\n" +
                      $"¡Gracias por tu compra en Farolito!/n/" +
                      $"Tu pedido está en proceso. Aquí están los detalles de tu compra:\n\n" +
                      $"Número de pedido: {ventaId}\n" +
                      $"Productos comprados: {productos}\n" +
                      $"Total de la compra: ${total:0.00}\n\n" +
                      $"Nos pondremos en contacto contigo cuando tu pedido sea procesado. \n\n" +
                      $"¡Gracias por elegirnos!\n\n" +
                      $"saludos,\n" +
                      $"El equipo de Farolito";
        
        await _emailService.EnviarCorreoAsync(usuario.Email, "Confirmación de tu compra en Farolito", mensaje);
    }
}