using Microsoft.AspNetCore.Routing.Constraints;
using System.Text.Json.Serialization;

namespace FarolitoAPIs.Models;

public partial class Solicitudproduccion
{
    public int Id { get; set; }

    public string? Descripcion { get; set; }
    public int Cantidad { get; set; }

    public int? Estatus { get; set; }

    public int RecetaId { get; set; }

    public int UsuarioId { get; set; }
    [JsonIgnore]
    public virtual ICollection<Produccion> Produccions { get; set; } = new List<Produccion>();

    public virtual Recetum Receta { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
