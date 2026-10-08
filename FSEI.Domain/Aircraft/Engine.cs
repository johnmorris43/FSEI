namespace FSEI.Domain.Aircraft;

public class Engine
{
    public int EngineId { get; set; }
    public int ManufacturerId { get; set; }
    public required Manufacturer Manufacturer { get; set; }
    public required string Model { get; set; }
    public string? Notes { get; set; }
    public ICollection<VariantEngine> VariantEngines { get; set; } = [];
}