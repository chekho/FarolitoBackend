using FarolitoAPIs.Data;
using FarolitoAPIs.Models;
using Microsoft.EntityFrameworkCore;

namespace FarolitoAPIs.Services;

public class CambioEstadoPedidoService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<CambioEstadoPedidoService> _logger;
    private readonly TimeSpan _intervaloBase = TimeSpan.FromSeconds(20);

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
            await Task.Delay(intervalo, stoppingToken);

            using var scope = _serviceScopeFactory.CreateScope();
            _logger.LogInformation("Scope creado para ejecutar el ciclo de procesamiento de pedidos.");

            try
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<FarolitoDbContext>();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                // Pedidos en camino
                var pedidosEnviados = await dbContext.Pedidos
                    .Where(p => p.FechaEnvio.HasValue && p.Estatus == "En Camino")
                    .Include(pedido => pedido.Ventum)
                    .ToListAsync(cancellationToken: stoppingToken);

                _logger.LogInformation($"Se encontraron {pedidosEnviados.Count} pedidos 'En Camino' para procesar.");
                foreach (var pedido in pedidosEnviados)
                {
                    if (await dbContext.PedidoNotificado.AnyAsync(
                            pn => pn.PedidoId == pedido.Id && pn.Estatus == "En Camino", stoppingToken))
                        continue;
                    
                    _logger.LogInformation($"Enviando correo para el pedido {pedido.Id} con estado 'En Camino'.");
                    var asunto = $"Estado actualizado para tu pedido {pedido.Id}";
                    var mensaje = $"Tu pedido {pedido.Id} ha sido enviado.";
                    try
                    {
                        await emailService.EnviarCorreoAsync(pedido.Ventum.UsuarioId, asunto, mensaje);

                        _logger.LogInformation(
                            $"Correo enviado exitosamente para el pedido {pedido.Id} con estado '{pedido.Estatus}'.");

                        dbContext.PedidoNotificado.Add(new PedidoNotificado
                        {
                            PedidoId = pedido.Id,
                            Estatus = "En Camino",
                            FechaNotificacion = DateTime.UtcNow
                        });
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error enviando correo para el pedido {pedido.Id}.");
                    }
                }

                var pedidosEntregados = await dbContext.Pedidos
                    .Where(p => p.FechaEntrega.HasValue && p.Estatus == "Finalizado")
                    .Include(p => p.Ventum)
                    .ToListAsync(cancellationToken: stoppingToken);
                _logger.LogInformation(
                    $"Se encontraron {pedidosEntregados.Count} pedidos 'Finalizados' para procesar.");

                foreach (var pedido in pedidosEntregados)
                {
                    if (await dbContext.PedidoNotificado.AnyAsync(pn => pn.PedidoId == pedido.Id && pn.Estatus == "Finalizado", stoppingToken))
                        continue;
                    
                    _logger.LogInformation($"Enviando correo para el pedido {pedido.Id} con estado 'Finalizado'.");
                    var asuntoEntrega = $"Tu pedido {pedido.Id} ha sido entregado";
                    var mensajeEntrega = $"¡Tu pedido {pedido.Id} ha sido entregado con éxito!";
                    try
                    {
                        await emailService.EnviarCorreoAsync(pedido.Ventum.UsuarioId, asuntoEntrega, mensajeEntrega);

                        _logger.LogInformation(
                            $"Correo enviado exitosamente para el pedido {pedido.Id} con estado '{pedido.Estatus}'.");

                        dbContext.PedidoNotificado.Add(new PedidoNotificado
                        {
                            PedidoId = pedido.Id,
                            Estatus = "Finalizado",
                            FechaNotificacion = DateTime.UtcNow
                        });
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error enviando correo para el pedido {pedido.Id}.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error procesando los pedidos.");
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
            > 100 => TimeSpan.FromSeconds(5),
            > 50 => TimeSpan.FromSeconds(10),
            > 20 => TimeSpan.FromSeconds(30),
            _ => _intervaloBase
        };
        
        return intervalo;
    }
}