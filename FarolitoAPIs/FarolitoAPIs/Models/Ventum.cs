using System.Text.Json.Serialization;

namespace FarolitoAPIs.Models;

public partial class Ventum
{
    public int Id { get; set; }

    public DateTime? Fecha { get; set; }

    public double? Descuento { get; set; }

    public string? Folio { get; set; }

    public string UsuarioId { get; set; }
    [JsonIgnore]
    public virtual ICollection<Detalleventum> Detalleventa { get; set; } = new List<Detalleventum>();

    public virtual Usuario Usuario { get; set; } = null!;
    public virtual Pedido Pedido { get; set; } = null!;
}
