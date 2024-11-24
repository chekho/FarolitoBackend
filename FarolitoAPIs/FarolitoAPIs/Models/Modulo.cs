using System.Text.Json.Serialization;

namespace FarolitoAPIs.Models
{
    public class Modulo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        [JsonIgnore]
        public virtual ICollection<Logs> Logs { get; set; } = new List<Logs>();
    }
}