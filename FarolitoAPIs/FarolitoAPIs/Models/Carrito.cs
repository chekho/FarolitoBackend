﻿namespace FarolitoAPIs.Models;

public class Carrito
{
    public int Id { get; set; }
    public int Cantidad { get; set; }
    public int RecetaId { get; set; }
    public string UsuarioId { get; set; }
    public DateTime UltimaActualizacion { get; set; }
    
    public virtual Recetum Receta { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
