namespace FarolitoAPIs.DTOs
{
    public class RecetaEstatusDTO
    {
        public int RecetaId { get; set; }
        public bool EstatusReceta { get; set; }
        public List<ComponenteEstatusDTO> Componentes { get; set; } = new List<ComponenteEstatusDTO>();
    }
}
