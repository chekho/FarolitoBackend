namespace FarolitoAPIs.Models;

public class PedidoNotificado
{
    public int Id { get; set; }
    public int PedidoId { get; set; }
    public bool PedidoEnviado { get; set; }
    public bool PedidoEntregado { get; set; }
    public DateTime FechaNotificacion { get; set; }
}
