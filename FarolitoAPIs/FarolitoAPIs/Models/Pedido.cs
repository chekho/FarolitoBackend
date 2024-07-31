using FarolitoAPIs.Models;

public partial class Pedido
{
    public int Id { get; set; }
    public DateOnly? FechaPedido { get; set; }
    public DateOnly? FechaEnvio { get; set; }
    public DateOnly? FechaEntrega { get; set; }

    public string? Estatus { get; set; }

    public int VentumId { get; set; }
    public virtual Ventum Ventum { get; set; } = null!;

}