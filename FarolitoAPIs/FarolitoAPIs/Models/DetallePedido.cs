namespace FarolitoAPIs.Models;

public partial class DetallePedido
{
    public int Id { get; set; }

    public int RecetaId { get; set; }

    public int PedidoId { get; set; }

    public virtual Pedido Pedido { get; set; } = null!;

    public virtual Recetum Receta { get; set; } = null!;
}
