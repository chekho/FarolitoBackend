﻿namespace FarolitoAPIs.DTOs
{
    public class ComentarioDTO
    {
        public int Id { get; set; }
        public string? Descripcion { get; set; }

        public DateTime? Fecha { get; set; }
        public string UserId { get; set; }
    }
}
