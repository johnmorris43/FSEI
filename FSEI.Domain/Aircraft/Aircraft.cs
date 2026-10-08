namespace FSEI.Domain.Aircraft;

public class Aircraft
{
    public int AircraftId { get; set; }
    public int ManufacturerId { get; set; }
    public required string Name { get; set; }
    public string? Notes { get; set; }
    public required Manufacturer Manufacturer { get; set; }
    public ICollection<AircraftVariant> Variants { get; set; } = [];
}