using FarolitoAPIs.Data;
using Microsoft.EntityFrameworkCore;

namespace FarolitoAPIs.Services;

public class CambioEstadoPedidoService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<CambioEstadoPedidoService> _logger;
    private readonly HashSet<(int PedidoId, string Estatus)> _pedidosNotificados;

    public CambioEstadoPedidoService(IServiceScopeFactory serviceScopeFactory,
        ILogger<CambioEstadoPedidoService> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
        _pedidosNotificados = new HashSet<(int, string)>();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(6000, stoppingToken);

            using var scope = _serviceScopeFactory.CreateScope();
            
            var dbContext = scope.ServiceProvider.GetRequiredService<FarolitoDbContext>();
            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

            var pedidosEnviados = await dbContext.Pedidos
                .Where(p => p.FechaEnvio.HasValue && p.Estatus == "En Camino")
                .Include(pedido => pedido.Ventum)
                .ToListAsync(cancellationToken: stoppingToken);

            foreach (var pedido in pedidosEnviados)
            {
                if (_pedidosNotificados.Contains((pedido.Id, "En Camino")))
                    continue;

                var asunto = $"Estado actualizado para tu pedido {pedido.Id}";
                var mensaje = $"Tu pedido {pedido.Id} ha sido enviado.";
                await emailService.EnviarCorreoAsync(pedido.Ventum.UsuarioId, asunto, mensaje);

                _pedidosNotificados.Add((pedido.Id, "En Camino"));
            }

            var pedidosEntregados = await dbContext.Pedidos
                .Where(p => p.FechaEntrega.HasValue && p.Estatus == "Finalizado")
                .Include(p => p.Ventum)
                .ToListAsync(cancellationToken: stoppingToken);

            foreach (var pedido in pedidosEntregados)
            {
                if (_pedidosNotificados.Contains((pedido.Id, "Finalizado")))
                    continue;

                var asuntoEntrega = $"Tu pedido {pedido.Id} ha sido entregado";
                var mensajeEntrega = $"¡Tu pedido {pedido.Id} ha sido entregado con éxito!";

                await emailService.EnviarCorreoAsync(pedido.Ventum.UsuarioId, asuntoEntrega, mensajeEntrega);

                _pedidosNotificados.Add((pedido.Id, "Finalizado"));
            }
        }
    }
}