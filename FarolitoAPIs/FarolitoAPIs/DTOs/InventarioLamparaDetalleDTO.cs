namespace FarolitoAPIs.DTOs
{
    public class InventarioLamparaDetalleDTO
    {
        public int Id { get; set; }
        public DateOnly? FechaProduccion { get; set; }
        public string Usuario { get; set; }
        public int? Cantidad { get; set; }
        public double? Precio { get; set; }
        public string? Estatus { get; set; }
    }
}
