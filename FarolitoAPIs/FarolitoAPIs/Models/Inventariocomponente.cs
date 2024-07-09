﻿using System;
using System.Collections.Generic;

namespace FarolitoAPIs.Models;

public partial class Inventariocomponente
{
    public int Id { get; set; }

    public int? Cantidad { get; set; }

    public int ProveedorId { get; set; }

    public int ComponentesId { get; set; }

    public int DetallecompraId { get; set; }

    public virtual Componente Componentes { get; set; } = null!;

    public virtual Detallecompra Detallecompra { get; set; } = null!;

    public virtual ICollection<Detalleproduccion> Detalleproduccions { get; set; } = new List<Detalleproduccion>();

    public virtual ICollection<Mermacomponente> Mermacomponentes { get; set; } = new List<Mermacomponente>();

    public virtual Proveedor Proveedor { get; set; } = null!;
}