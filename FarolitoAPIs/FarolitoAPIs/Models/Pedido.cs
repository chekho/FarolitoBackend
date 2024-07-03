using System;
using System.Collections.Generic;

namespace FarolitoAPIs.Models;

public partial class Pedido
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }

    public int UsuarioDetallesUsuarioId { get; set; }

    public string? Fecha { get; set; }

    public byte? Estatus { get; set; }

    public virtual ICollection<DetallePedido> DetallePedidos { get; set; } = new List<DetallePedido>();

    public virtual Usuario Usuario { get; set; } = null!;

    public virtual DetallesUsuario UsuarioDetallesUsuario { get; set; } = null!;
}
