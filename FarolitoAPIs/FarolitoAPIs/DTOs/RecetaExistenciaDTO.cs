namespace FarolitoAPIs.DTOs
{
    public class RecetaExistenciaDTO
    {
        public int RecetaId { get; set; }
        public string Nombre { get; set; }
        public int TotalCantidad { get; set; }
        public bool Existe { get; set; }
    }
}
