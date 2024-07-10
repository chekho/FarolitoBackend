using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FarolitoAPIs.Models;

public partial class Produccion
{
    public int Id { get; set; }

    public DateOnly? Fecha { get; set; }

    public double? Costo { get; set; }

    public int UsuarioId { get; set; }

    public int SolicitudproduccionId { get; set; }
    [JsonIgnore]
    public virtual ICollection<Detalleproduccion> Detalleproduccions { get; set; } = new List<Detalleproduccion>();
    [JsonIgnore]
    public virtual ICollection<Inventariolampara> Inventariolamparas { get; set; } = new List<Inventariolampara>();

    public virtual Solicitudproduccion Solicitudproduccion { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
