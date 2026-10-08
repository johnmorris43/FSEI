namespace FSEI.Domain.Aircraft;

public class VariantEngine
{
    public int VariantEngineId { get; set; }
    public int AircraftVariantId { get; set; }
    public int EngineId { get; set; }
    public string? Notes { get; set; }
    public required AircraftVariant AircraftVariant { get; set; }
    public required Engine Engine { get; set; }
}