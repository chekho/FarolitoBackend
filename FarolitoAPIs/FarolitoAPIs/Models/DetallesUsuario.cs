using System;
using System.Collections.Generic;

namespace FarolitoAPIs.Models;

public partial class DetallesUsuario
{
    public int Id { get; set; }

    public string? Nombres { get; set; }

    public string? ApellidoM { get; set; }

    public string? ApellidoP { get; set; }

    public string? Correo { get; set; }

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
