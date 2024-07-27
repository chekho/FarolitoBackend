using Microsoft.CodeAnalysis.CodeStyle;

namespace FarolitoAPIs.DTOs
{
    public class RecetaConDetallesDTO
    {
        public string Nombrelampara { get; set; }
        public int Existencias { get; set; }
        public double Costo { get; set; }
        public List<InventarioLamparaDetalleDTO> Detalles { get; set; }
    }
}
