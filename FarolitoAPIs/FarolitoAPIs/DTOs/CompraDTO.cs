namespace FarolitoAPIs.DTOs
{
    public class CompraDTO
    {
        public int Id { get; set; }
        public DateOnly? Fecha { get; set; }
        public string UsuarioNombre { get; set; } 
        public List<DetalleCompraDTO> Detalles { get; set; } = new List<DetalleCompraDTO>();
    }
}
