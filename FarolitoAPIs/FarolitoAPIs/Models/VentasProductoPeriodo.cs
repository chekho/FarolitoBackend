namespace FarolitoAPIs.Models
{
    public class VentasProductoPeriodo
    {
        public int Id {  get; set; }
        public int Anio { get; set; }
        public int Mes { get; set; }
        public string Producto { get; set; }
        public int NumeroDeVentas { get; set; }
    }
}
