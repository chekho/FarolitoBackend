namespace FarolitoAPIs.Models;

public partial class Carrito
{
    public int Id { get; set; }

    public DateOnly? Fecha { get; set; }

    public byte? Stastus { get; set; }
    public int Cantidad { get; set; }
    public int InventarioLamparaId { get; set; }

    public string UsuarioId { get; set; }

    public virtual Inventariolampara Inventariolampara { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
