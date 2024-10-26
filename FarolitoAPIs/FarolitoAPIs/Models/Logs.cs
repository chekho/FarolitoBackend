using System.Text.Json.Serialization;

namespace FarolitoAPIs.Models
{
    public class Logs
    {//fec,a hora, usuario, descripción del cambio y módulo
        public int Id { get; set; }
        public DateTime FechaHora { get; set; }
        public string Cambio { get; set; } = "";
        public int UsuarioId { get; set; }
        public int ModuloId { get; set; }

        [JsonIgnore]
        public virtual Usuario Usuario { get; set; } = null!;
        [JsonIgnore]
        public virtual Modulo Modulo { get; set; } = null!;
    }
}
