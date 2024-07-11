using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FarolitoAPIs.Models;

public partial class DetallesUsuario
{
    [JsonIgnore]
    public int Id { get; set; }

    public string? Nombres { get; set; }

    public string? ApellidoM { get; set; }

    public string? ApellidoP { get; set; }

    public string? Correo { get; set; }
    [JsonIgnore]
    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    [JsonIgnore]
    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
