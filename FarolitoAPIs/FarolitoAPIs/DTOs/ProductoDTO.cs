namespace FarolitoAPIs.DTOs
{
    public class ProductoDTO
    {
        public int RecetaId { get; set; }
        public int InventarioId { get; set; }
        public int Cantidad { get; set; }
        public double PrecioUnitario { get; set; }
        public string NombreProducto { get; set; }
    }
}
