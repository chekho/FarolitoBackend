using System;
using System.Collections.Generic;

namespace FarolitoAPIs.Models;

public partial class Recetum
{
    public int Id { get; set; }

    public string? Nombrelampara { get; set; }

    public int? Estatus { get; set; }

    public virtual ICollection<Carrito> Carritos { get; set; } = new List<Carrito>();

    public virtual ICollection<Componentesrecetum> Componentesreceta { get; set; } = new List<Componentesrecetum>();

    public virtual ICollection<DetallePedido> DetallePedidos { get; set; } = new List<DetallePedido>();

    public virtual ICollection<Inventariolampara> Inventariolamparas { get; set; } = new List<Inventariolampara>();

    public virtual ICollection<Solicitudproduccion> Solicitudproduccions { get; set; } = new List<Solicitudproduccion>();
}
