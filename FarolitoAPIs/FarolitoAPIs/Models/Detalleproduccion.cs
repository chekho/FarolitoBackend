namespace FarolitoAPIs.Models;

public partial class Detalleproduccion
{
    public int Id { get; set; }

    public int ProduccionId { get; set; }

    public int InventariocomponentesId { get; set; }

    public virtual Inventariocomponente Inventariocomponentes { get; set; } = null!;

    public virtual Produccion Produccion { get; set; } = null!;
}
