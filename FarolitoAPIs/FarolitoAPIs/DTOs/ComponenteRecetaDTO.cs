namespace FarolitoAPIs.DTOs
{
    public class ComponenteRecetaDTO
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public int? Cantidad { get; set; }
        public bool? Estatus { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal PrecioTotal { get; set; }
    }
}
