namespace FarolitoAPIs.Models;

public partial class Componentesrecetum
{
    public int Id { get; set; }

    public int? Cantidad { get; set; }

    public bool? Estatus { get; set; }

    public int RecetaId { get; set; }

    public int ComponentesId { get; set; }

    public virtual Componente Componentes { get; set; } = null!;

    public virtual Recetum Receta { get; set; } = null!;
}
