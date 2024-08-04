namespace FarolitoAPIs.Models
{
    public class LamparaCliente
    {
        public int Id { get; set; }
        public string Cliente { get; set; }
        public string Producto { get; set; }
        public int NumeroDeVentas { get; set; }
        public double TotalGastado { get; set; }
    }
}
