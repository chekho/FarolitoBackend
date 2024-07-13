using System.Text.Json.Serialization;

namespace FarolitoAPIs.Models;

public partial class Proveedor
{
    public int Id { get; set; }

    public string? NombreEmpresa { get; set; }

    public string? Dirección { get; set; }

    public string? Teléfono { get; set; }

    public string? NombreAtiende { get; set; }

    public string? ApellidoM { get; set; }

    public string? ApellidoP { get; set; }

    public byte? Estatus { get; set; }
    [JsonIgnore]
    public virtual ICollection<Inventariocomponente> Inventariocomponentes { get; set; } = new List<Inventariocomponente>();
    [JsonIgnore]
    public virtual ICollection<Productoproveedor> Productoproveedors { get; set; } = new List<Productoproveedor>();
}
