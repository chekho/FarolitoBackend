namespace FarolitoAPIs.Models;

public partial class Detalleventum
{
    public int Id { get; set; }

    public int? Cantidad { get; set; }

    public double? PrecioUnitario { get; set; }

    public int VentaId { get; set; }

    public int InventariolamparaId { get; set; }

    public virtual Inventariolampara Inventariolampara { get; set; } = null!;

    public virtual Ventum Venta { get; set; } = null!;
}
