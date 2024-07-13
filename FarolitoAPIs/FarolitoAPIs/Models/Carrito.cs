namespace FarolitoAPIs.Models;

public partial class Carrito
{
    public int Id { get; set; }

    public DateOnly? Fecha { get; set; }

    public byte? Stastus { get; set; }

    public int RecetaId { get; set; }

    public int UsuarioId { get; set; }

    public virtual Recetum Receta { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
