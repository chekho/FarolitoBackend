using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FarolitoAPIs.Models;

public partial class Componente
{
    public int Id { get; set; }

    public string? Nombre { get; set; }
    [JsonIgnore]
    public virtual ICollection<Componentesrecetum> Componentesreceta { get; set; } = new List<Componentesrecetum>();
    [JsonIgnore]
    public virtual ICollection<Inventariocomponente> Inventariocomponentes { get; set; } = new List<Inventariocomponente>();
    [JsonIgnore]
    public virtual ICollection<Productoproveedor> Productoproveedors { get; set; } = new List<Productoproveedor>();
}
