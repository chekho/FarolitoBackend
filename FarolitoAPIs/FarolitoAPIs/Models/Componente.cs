using System;
using System.Collections.Generic;

namespace FarolitoAPIs.Models;

public partial class Componente
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public virtual ICollection<Componentesrecetum> Componentesreceta { get; set; } = new List<Componentesrecetum>();

    public virtual ICollection<Inventariocomponente> Inventariocomponentes { get; set; } = new List<Inventariocomponente>();

    public virtual ICollection<Productoproveedor> Productoproveedors { get; set; } = new List<Productoproveedor>();
}
