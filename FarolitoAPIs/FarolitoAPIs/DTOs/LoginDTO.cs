﻿using System.ComponentModel.DataAnnotations;

namespace FarolitoAPIs.DTOs
{
    public class LoginDTO
    {
        [EmailAddress]
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }

        public string? RecaptchaToken { get; set; }
    }
}
