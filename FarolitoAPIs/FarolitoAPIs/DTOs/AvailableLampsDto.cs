namespace FarolitoAPIs.DTOs;

public class AvailableLampsDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public required int AvailableQuantity { get; set; }
    public required double AveragePrice { get; set; }
    
}