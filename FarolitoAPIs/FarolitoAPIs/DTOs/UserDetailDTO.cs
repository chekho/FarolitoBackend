﻿namespace FarolitoAPIs.DTOs
{
    public class UserDetailDTO
    {
        public string? Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Tarjeta { get; set; }
        public string[]? Roles { get; set; }
        public string? PhoneNumber { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public int AccessFailedCount { get; set; }
        public string? UrlImage { get; set; }
        public string? Direccion { get; set; }
        public string? Facebook {  get; set; }
    }
}
