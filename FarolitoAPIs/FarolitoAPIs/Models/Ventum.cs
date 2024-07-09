﻿using System;
using System.Collections.Generic;

namespace FarolitoAPIs.Models;

public partial class Ventum
{
    public int Id { get; set; }

    public DateTime? Fecha { get; set; }

    public double? Descuento { get; set; }

    public string? Folio { get; set; }

    public int UsuarioId { get; set; }

    public virtual ICollection<Detalleventum> Detalleventa { get; set; } = new List<Detalleventum>();

    public virtual Usuario Usuario { get; set; } = null!;
}