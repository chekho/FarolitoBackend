namespace FarolitoAPIs.DTOs;

public class OrderStatusDto
{
    public int OrderId { get; set; }
    public string? State { get; set; }
    public DateOnly? OrderDate { get; set; }
    public DateOnly? ShippingDate { get; set; }
    public DateOnly? DeliveryDate { get; set; }
}