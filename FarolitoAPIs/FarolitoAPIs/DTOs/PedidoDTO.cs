namespace FarolitoAPIs.DTOs
{
    public class PedidoDTO
    {
        public int Id { get; set; }
        public DateOnly? FechaPedido { get; set; }
        public DateOnly? FechaEnvio { get; set; }
        public DateOnly? FechaEntrega { get; set; }
        public DateOnly? FechaEntregaAprox { get; set; }
        public string Estatus { get; set; }
        public List<ProductoDTO> Productos { get; set; }
    }
}
