using System.Text.Json.Serialization;

namespace FarolitoAPIs.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? Contraseña { get; set; }

    public string? Token { get; set; }

    public string? Rol { get; set; }

    public byte? Estatus { get; set; }
    [JsonIgnore]

    public int DetallesUsuarioId { get; set; }
    [JsonIgnore]
    public virtual ICollection<Carrito> Carritos { get; set; } = new List<Carrito>();
    [JsonIgnore]
    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();
    public virtual DetallesUsuario DetallesUsuario { get; set; } = null!;
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
}
