namespace FarolitoAPIs.DTOs
{
    public class RecetaDetalle2DTO
    {
        public int Id { get; set; }
        public string? Nombrelampara { get; set; }
        public bool? Estatus { get; set; }
        public List<ComponenteReceta2DTO> Componentes { get; set; } = new List<ComponenteReceta2DTO>();
    }
}
