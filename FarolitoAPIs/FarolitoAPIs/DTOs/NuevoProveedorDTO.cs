using System.ComponentModel;

namespace FarolitoAPIs.DTOs
{
    public class NuevoProveedorDTO
    {
        public int Id { get; set; }
        public string NombreEmpresa { get; set; }
        public string Dirección { get; set; }
        public string Teléfono { get; set; }
        public string NombreAtiende { get; set; }
        public string ApellidoM { get; set; }
        public string ApellidoP { get; set; }
        public bool Estatus { get; set; }
        public List<ComponenteDTO> Productos { get; set; }
    }
}
