namespace FarolitoAPIs.Models
{
    public class VentaProducto
    {
        public int Id { get; set; }
        public string Producto { get; set; }
        public int NumeroDeVentas { get; set; }
        public double TotalRecaudado { get; set; }
    }
}
