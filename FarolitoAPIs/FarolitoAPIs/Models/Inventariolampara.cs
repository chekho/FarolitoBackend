using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FarolitoAPIs.Models;

public partial class Inventariolampara
{
    public int Id { get; set; }

    public int? Cantidad { get; set; }

    public double? Precio { get; set; }

    public DateOnly? FechaCreacion { get; set; }

    public string? Lote { get; set; }

    public int RecetaId { get; set; }

    public int ProduccionId { get; set; }
    [JsonIgnore]
    public virtual ICollection<Detalleventum> Detalleventa { get; set; } = new List<Detalleventum>();
    [JsonIgnore]
    public virtual ICollection<Mermalampara> Mermalamparas { get; set; } = new List<Mermalampara>();

    public virtual Produccion Produccion { get; set; } = null!;

    public virtual Recetum Receta { get; set; } = null!;
}
