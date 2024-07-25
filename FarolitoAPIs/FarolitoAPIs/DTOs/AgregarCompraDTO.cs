namespace FarolitoAPIs.DTOs
{
    public class AgregarCompraDTO
    {
        public DateOnly Fecha { get; set; }
        public int ProveedorId { get; set; }
        public List<DetalleCompra2DTO> Detalles { get; set; }
    }
}
