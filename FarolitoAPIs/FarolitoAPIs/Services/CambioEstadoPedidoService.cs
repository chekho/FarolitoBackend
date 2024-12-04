using FarolitoAPIs.Data;
using FarolitoAPIs.Models;
using Microsoft.EntityFrameworkCore;

namespace FarolitoAPIs.Services;

public class CambioEstadoPedidoService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<CambioEstadoPedidoService> _logger;
    private readonly TimeSpan _intervaloBase = TimeSpan.FromSeconds(60);

    public CambioEstadoPedidoService(IServiceScopeFactory serviceScopeFactory,
        ILogger<CambioEstadoPedidoService> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("[Inicio] Procesando cambios de estado de pedidos...");

            TimeSpan intervalo = await CalcularIntervaloAsync(stoppingToken);

            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                _logger.LogInformation("Scope creado para ejecutar el ciclo de procesamiento de pedidos.");

                var dbContext = scope.ServiceProvider.GetRequiredService<FarolitoDbContext>();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                await ProcesarPedidosAsync(dbContext, emailService, stoppingToken, "En Camino",
                    "Estado actualizado para tu pedido", "Tu pedido {0} ha sido enviado.", true, false);

                await ProcesarPedidosAsync(dbContext, emailService, stoppingToken, "Finalizado",
                    "Tu pedido {0} ha sido entregado", "¡Tu pedido {0} ha sido entregado con éxito!", false, true);

                await dbContext.SaveChangesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error procesando los pedidos.");
            }

            await Task.Delay(intervalo, stoppingToken);
        }
    }

    private async Task ProcesarPedidosAsync(FarolitoDbContext dbContext, IEmailService emailService,
        CancellationToken stoppingToken, string estadoPedido, string asunto, string mensaje, bool enviado,
        bool entregado)
    {
        var pedidos = await dbContext.Pedidos
            .Where(p => p.Estatus == estadoPedido &&
                        (estadoPedido == "En Camino" ? p.FechaEnvio.HasValue : p.FechaEntrega.HasValue))
            .Include(p => p.Ventum)
            .ToListAsync(cancellationToken: stoppingToken);

        _logger.LogInformation($"Se encontraron {pedidos.Count} pedidos '{estadoPedido}' para procesar.");

        foreach (var pedido in pedidos)
        {
            if (await dbContext.PedidoNotificado.AnyAsync(
                    pn => pn.PedidoId == pedido.Id && (enviado ? pn.PedidoEnviado : pn.PedidoEntregado),
                    stoppingToken)) continue;

            _logger.LogInformation($"Enviando correo para el pedido {pedido.Id} con estado '{estadoPedido}'.");

            var mensajeFinal = string.Format(mensaje, pedido.Id);

            try
            {
                await emailService.EnviarCorreoAsync(pedido.Ventum.UsuarioId, asunto, mensajeFinal);

                _logger.LogInformation(
                    $"Correo enviado exitosamente para el pedido {pedido.Id} con estado '{estadoPedido}'.");

                var pedidoNotificado =
                    await dbContext.PedidoNotificado.FirstOrDefaultAsync(pn => pn.PedidoId == pedido.Id,
                        cancellationToken: stoppingToken);

                if (pedidoNotificado != null)
                {
                    if (enviado)
                    {
                        pedidoNotificado.PedidoEnviado = true;
                    }
                    else
                    {
                        pedidoNotificado.PedidoEntregado = true;
                    }

                    pedidoNotificado.FechaNotificacion = DateTime.UtcNow;
                }
                else
                {
                    dbContext.PedidoNotificado.Add(new PedidoNotificado
                    {
                        PedidoId = pedido.Id,
                        PedidoEnviado = enviado,
                        PedidoEntregado = entregado,
                        FechaNotificacion = DateTime.UtcNow
                    });
                }
                
                var historialComunicacion = new HistorialComunicacion
                {
                    AccionRealizada = $"Se envió un correo sobre la actualización del estado de su pedido {pedido.Id}",
                    Fecha = DateTime.UtcNow,
                    UsuarioId = pedido.Ventum.UsuarioId,
                };

                dbContext.HistorialComunicaciones.Add(historialComunicacion);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error enviando correo para el pedido {pedido.Id}.");
            }
        }
    }

    private async Task<TimeSpan> CalcularIntervaloAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<FarolitoDbContext>();

        var pedidosPendientesCount = await dbContext.Pedidos
            .Where(p => p.Estatus == "En Camino" || p.Estatus == "Finalizado")
            .CountAsync(cancellationToken: stoppingToken);

        var intervalo = pedidosPendientesCount switch
        {
            > 100 => TimeSpan.FromSeconds(10),
            > 50 => TimeSpan.FromSeconds(20),
            > 20 => TimeSpan.FromSeconds(40),
            _ => _intervaloBase
        };

        return intervalo;
    }
}