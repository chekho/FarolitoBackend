namespace FarolitoAPIs.DTOs
{
    public class DetalleSolicitudProduccionDTO
    {
        public int Id { get; set; }
        public string? Descripcion { get; set; }
        public int Cantidad { get; set; }
        public string Estatus { get; set; }
        public string NombreUsuario { get; set; }
        public DetalleRecetaDTO Receta { get; set; }
    }

    public class DetalleRecetaDTO
    {
        public int Id { get; set; }
        public string Nombrelampara { get; set; }
    }
}
