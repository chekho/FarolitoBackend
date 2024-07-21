using System.ComponentModel.DataAnnotations;

namespace FarolitoAPIs.DTOs
{
    public class ForgotPasswordDTO
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
    }
}
