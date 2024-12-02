using System.ComponentModel.DataAnnotations;

namespace FarolitoAPIs.DTOs
{
    public class UpdateUserDTO
    {
        public string? FullName { get; set; }
        
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string Direccion { get; set; }

        public string? Facebook {  get; set; }
    }
}
