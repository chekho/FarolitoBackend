using FarolitoAPIs.Data;
using Microsoft.EntityFrameworkCore;

namespace FarolitoAPIs.Services;

public class CarritoService : BackgroundService
{
    private readonly FarolitoDbContext _dbContext;
    private readonly IEmailService _emailService;
    private readonly ILogger<CarritoService> _logger;

    public CarritoService(FarolitoDbContext dbContext, IEmailService emailService, ILogger<CarritoService> logger)
    {
        _dbContext = dbContext;
        _emailService = emailService;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var tiempoLimite = DateTime.UtcNow.AddHours(-24);
                var carritosAbandonados = await _dbContext.Carritos
                    .Include(c => c.Usuario)
                    .Where(c => c.UltimaActualizacion < tiempoLimite)
                    .GroupBy(c => c.UsuarioId)
                    .Where(grupo => grupo.Any())
                    .ToListAsync(stoppingToken);

                foreach (var grupoCarrito in carritosAbandonados)
                {
                    var usuario = grupoCarrito.First().Usuario;
                    if (usuario == null) continue;

                    var items = string.Join(", ", grupoCarrito.Select(c => c.Receta.Nombrelampara));
                    var mensaje =
                        $"Hola {usuario.FullName}, notamos que dejaste estos artículos en tu carrito: {items}. ¡Vuelve y completa tu compra!";

                    await _emailService.EnviarCorreoAsync(usuario.Email, "No olvides completar tu compra", mensaje);
                }

                foreach (var grupoCarrito in carritosAbandonados)
                {
                    foreach (var carrito in grupoCarrito)
                    {
                        carrito.UltimaActualizacion = DateTime.UtcNow;
                    }
                }

                await _dbContext.SaveChangesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error procesando carritos abandonados.");
            }
            
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}