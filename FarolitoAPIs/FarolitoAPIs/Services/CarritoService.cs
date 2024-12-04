using FarolitoAPIs.Data;
using FarolitoAPIs.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace FarolitoAPIs.Services;

public class CarritoService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<CarritoService> _logger;

    public CarritoService(IServiceScopeFactory serviceScopeFactory, ILogger<CarritoService> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("[Inicio] Procesando carrito");
            try
            {
                await ProcesarCarritosAbandonadosAsync(stoppingToken);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Procesamiento de carrito cancelado por solicitud.");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado procesando carritos abandonados.");
            }
            finally
            {
                try
                {
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("Tarea retrasada cancelada.");
                }
            }
        }
    }
    
    private async Task ProcesarCarritosAbandonadosAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("[Inicio] Procesando carritos abandonados.");

        using var scope = _serviceScopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<FarolitoDbContext>();
        var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

        var tiempoLimite = DateTime.UtcNow.AddMinutes(-10);

        try
        {
            var carritosAbandonados = (await dbContext.Carritos
                .Include(c => c.Usuario)
                .Where(c => c.UltimaActualizacion < tiempoLimite)
                .ToListAsync(stoppingToken))
                .GroupBy(c => c.UsuarioId)
                .Where(grupo => grupo.Any())
                .ToList();

            foreach (var grupoCarrito in carritosAbandonados)
            {
                var usuario = grupoCarrito.First().Usuario;
                if (usuario?.Email == null) continue;

                var items = string.Join(", ", 
                    !grupoCarrito.IsNullOrEmpty() ? 
                        grupoCarrito.Select(c => c.Receta.Nombrelampara) :
                        "Nombre no disponible");
                
                var mensaje =
                    $"Hola {usuario.FullName}, notamos que dejaste estos artículos en tu carrito: {items}. ¡Vuelve y completa tu compra!";

                try
                {
                    await emailService.EnviarCorreoAsync(usuario.Email, "No olvides completar tu compra", mensaje);
                    _logger.LogInformation($"Correo enviado a {usuario.Email} para el usuario {usuario.FullName}.");

                    var historialComunicacion = new HistorialComunicacion
                    {
                        AccionRealizada = "Se envió un correo de recordatorio sobre carrito abandonado",
                        Fecha = DateTime.UtcNow,
                        UsuarioId = usuario.Id,
                    };

                    dbContext.HistorialComunicaciones.Add(historialComunicacion);
                }
                catch (Exception emailEx)
                {
                    _logger.LogError(emailEx, $"Error enviando correo a {usuario.Email}.");
                }
            }

            foreach (var grupoCarrito in carritosAbandonados)
            {
                foreach (var carrito in grupoCarrito)
                {
                    carrito.UltimaActualizacion = DateTime.UtcNow;
                }
            }

            await dbContext.SaveChangesAsync(stoppingToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Operación cancelada durante el procesamiento de carritos.");
            throw; // Propaga para ser manejada en el bucle principal.
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error procesando carritos abandonados.");
        }
    }
}