namespace FarolitoAPIs.DTOs
{
    public class DetalleCompraDTO
    {
        public int Id { get; set; }
        public int? Cantidad { get; set; }
        public double? Costo { get; set; }
        public string NombreComponente { get; set; }
    }
}
