namespace FSEI.Domain.Aircraft;

public class Variant
{
    public int VariantId { get; set; }
    public int AircraftId { get; set; }
    public required string Name { get; set; }
    public string? Notes { get; set; }
    public required Aircraft Aircraft { get; set; }   
    public ICollection<AircraftConfiguration> AircraftConfigurations { get; set; } = [];
}