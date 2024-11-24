namespace FarolitoAPIs.DTOs
{
    public class VentaProductoPeriodoDTO
    {
        public int Año { get; set; }
        public int Mes { get; set; }
        public int NumeroVentas { get; set; }
        public string Producto { get; set; }
        public decimal TotalRecaudado { get; set; }
    }
}
