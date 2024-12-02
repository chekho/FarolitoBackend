namespace FarolitoAPIs.Models;

public class HistorialComunicacion
{
    public int Id { get; set; }
    public required string AccionRealizada { get; set; }
    public required DateTime Fecha { get; set; }

    public string UsuarioId { get; set; }
    
    public virtual Usuario usuario { get; set; } = null!;
}