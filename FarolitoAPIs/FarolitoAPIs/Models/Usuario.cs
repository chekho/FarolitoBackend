﻿using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace FarolitoAPIs.Models;

public partial class Usuario : IdentityUser
{
    public string? FullName { get; set; }
    public string? urlImage { get; set; }
    public string? Direccion { get; set; }
    public string? Tarjeta { get; set; }
    public string? Facebook { get; set; }
    public DateTime? LastLogin { get; set; }

    [JsonIgnore]
    public virtual ICollection<Carrito> Carritos { get; set; } = new List<Carrito>();
    [JsonIgnore]
    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();
    [JsonIgnore]
    public virtual ICollection<Mermacomponente> Mermacomponentes { get; set; } = new List<Mermacomponente>();
    [JsonIgnore]
    public virtual ICollection<Mermalampara> Mermalamparas { get; set; } = new List<Mermalampara>();
    [JsonIgnore]
    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    [JsonIgnore]
    public virtual ICollection<Produccion> Produccions { get; set; } = new List<Produccion>();
    [JsonIgnore]
    public virtual ICollection<Solicitudproduccion> Solicitudproduccions { get; set; } = new List<Solicitudproduccion>();
    [JsonIgnore]
    public virtual ICollection<Ventum> Venta { get; set; } = new List<Ventum>();

    [JsonIgnore]
    public virtual ICollection<Logs> Logs { get; set; } = new List<Logs>();


    [JsonIgnore]
    public virtual ICollection<HistorialComunicacion> HistorialComunicaciones { get; set; } =
        new List<HistorialComunicacion>();
}
