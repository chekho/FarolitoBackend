using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FarolitoAPIs.Models;

public partial class Detallecompra
{
    public int Id { get; set; }

    public int? Cantidad { get; set; }

    public string? Lote { get; set; }

    public double? Costo { get; set; }

    public int CompraId { get; set; }

    public virtual Compra Compra { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<Inventariocomponente> Inventariocomponentes { get; set; } = new List<Inventariocomponente>();
}
