using System.ComponentModel.DataAnnotations;

namespace FarolitoAPIs.DTOs
{
    public class ComponenteDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
    }
}
