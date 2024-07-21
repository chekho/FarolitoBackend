using System.ComponentModel.DataAnnotations;

namespace FarolitoAPIs.DTOs
{
    public class UpdateUserDTO
    {
        [Required]
        public string? FullName { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [Phone]
        public string? PhoneNumber { get; set; }
    }
}
