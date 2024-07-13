namespace FarolitoAPIs.Models;

public partial class Mermalampara
{
    public int Id { get; set; }

    public int? Cantidad { get; set; }

    public string? Descripcion { get; set; }

    public DateOnly? Fecha { get; set; }

    public int UsuarioId { get; set; }

    public int InventariolamparaId { get; set; }

    public virtual Inventariolampara Inventariolampara { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
