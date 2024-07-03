using System;
using System.Collections.Generic;

namespace FarolitoAPIs.Models;

public partial class Detallecompra
{
    public int Id { get; set; }

    public int? Cantidad { get; set; }

    public string? Lote { get; set; }

    public double? Costo { get; set; }

    public int CompraId { get; set; }

    public virtual Compra Compra { get; set; } = null!;

    public virtual ICollection<Inventariocomponente> Inventariocomponentes { get; set; } = new List<Inventariocomponente>();
}
