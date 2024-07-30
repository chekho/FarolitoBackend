using System.ComponentModel.DataAnnotations;

namespace FarolitoAPIs.DTOs
{
    public class ComponentesRequestDTO
    {
        [Required]
        public int Id { get; set; }
        public int Cantidad { get; set; }
    }
}
