namespace FarolitoAPIs.Models
{
    public class MermaProveedor
    {
        public int Id { get; set; }
        public int Mermado { get; set; }
        public int Comprado { get; set; }
        public decimal TotalMermado { get; set; }
        public decimal Costo { get; set; }
        public float PorcentajeMermado { get; set; }
        public string NombreEmpresa { get; set; }
    }
}
