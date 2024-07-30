using System.Text.Json.Serialization;

namespace FarolitoAPIs.Models;

public partial class Mermalampara
{
    public int Id { get; set; }

    public int? Cantidad { get; set; }

    public string? Descripcion { get; set; }

    public DateOnly? Fecha { get; set; }

    public string UsuarioId { get; set; }

    public int InventariolamparaId { get; set; }
    [JsonIgnore]
    public virtual Inventariolampara Inventariolampara { get; set; } = null!;
    [JsonIgnore]
    public virtual Usuario Usuario { get; set; } = null!;
}
