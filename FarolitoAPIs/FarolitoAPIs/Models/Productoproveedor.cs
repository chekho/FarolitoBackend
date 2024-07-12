using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FarolitoAPIs.Models;

public partial class Productoproveedor
{
    public int Id { get; set; }

    public int ProveedorId { get; set; }

    public int ComponentesId { get; set; }
    public virtual Componente Componentes { get; set; } = null!;
    public virtual Proveedor Proveedor { get; set; } = null!;
}
