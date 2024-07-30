namespace FarolitoAPIs.DTOs
{
    public class DetallesProduccionDTO
    {
        public int Id { get; set; }
        public DateOnly? Fecha { get; set; }
        public double? Costo { get; set; }
        public string NombreUsuario { get; set; }
        public DetalleSolicitudProduccionDTO SolicitudProduccion { get; set; }
    }
}
