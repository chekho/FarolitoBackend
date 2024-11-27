namespace FarolitoAPIs.DTOs;

public class RequiredComponentsDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int? RequiredAmount { get; set; }
    public required string Supplier { get; set; }
}