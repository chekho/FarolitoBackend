namespace FarolitoAPIs.DTOs
{
    public class InventarioComponenteDetalleDTO
    {
        public int Id { get; set; }
        public int? Cantidad { get; set; }
        public string ProveedorNombre { get; set; } // Nombre del proveedor
        public DateOnly FechaCompra { get; set; } // Fecha de compra
        public double? PrecioUnitario { get; set; }
    }

}
