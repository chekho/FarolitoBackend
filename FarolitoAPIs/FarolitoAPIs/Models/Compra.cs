using System.Text.Json.Serialization;

namespace FarolitoAPIs.Models;

public partial class Compra
{
    public int Id { get; set; }

    public DateOnly? Fecha { get; set; }

    public int UsuarioId { get; set; }
    [JsonIgnore]
    public virtual ICollection<Detallecompra> Detallecompras { get; set; } = new List<Detallecompra>();

    public virtual Usuario Usuario { get; set; } = null!;
}
