namespace FarolitoAPIs.DTOs
{
    public class MermaLamparaDTO
    {
        public int Cantidad { get; set; }
        public string Descripcion { get; set; }
        public int InventariolamparaId { get; set; }
    }

    public class MermaComponenteDTO
    {
        public int Cantidad { get; set; }
        public string Descripcion { get; set; }
        public int InventarioComponenteId { get; set; }
    }
    
    public class DetalleMermaComponenteDTO
    {
        public int Id { get; set; }
        public int? Cantidad { get; set; }
        public string Descripcion { get; set; }
        public DateOnly? Fecha { get; set; }
        public string Usuario { get; set; }
        public string Componente { get; set; }

    }

    public class DetalleMermaLamparaDTO
    {
        public int Id { get; set; }
        public int? Cantidad { get; set; }
        public string Descripcion { get; set; }
        public DateOnly? Fecha { get; set; }
        public string? Usuario { get; set; }
        public string? Lampara { get; set; }

    }
}
