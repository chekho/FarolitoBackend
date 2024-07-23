using System.ComponentModel.DataAnnotations;

namespace FarolitoAPIs.DTOs
{
    public class PatchComponenteDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public bool? estatus { get; set; }
    }
}
