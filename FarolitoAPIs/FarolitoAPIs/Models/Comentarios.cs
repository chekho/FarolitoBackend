using System.Text.Json.Serialization;

namespace FarolitoAPIs.Models
{
    public class Comentarios
    {
        public int Id { get; set; }
        public string? Descripcion { get; set; }

        public DateTime? Fecha { get; set; }
        public string UserId { get; set; }

        [JsonIgnore]
        public virtual Usuario Usuario { get; set; } = null!;
    }
}
