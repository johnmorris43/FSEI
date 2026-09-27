namespace FSEI.Domain.Aircraft;

public class VariantEngine
{
    public int VariantEngineId { get; set; }
    public int VariantId { get; set; }
    public int EngineId { get; set; }
    public string? Notes { get; set; }
    public required Variant Variant { get; set; }
    public required Engine Engine { get; set; }
}