namespace FarolitoAPIs.DTOs
{
    public class ComponenteConDetallesDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Existencia { get; set; }

        public List<InventarioComponenteDetalleDTO> Detalles { get; set; }
    }
}
