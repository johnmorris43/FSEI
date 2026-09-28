using FSEI.Domain.Avionics;

namespace FSEI.Domain.Aircraft;



public class AircraftConfiguration
{
    public int AircraftConfigurationId { get; set; }

    public int VariantId { get; set; }

    public required string Name { get; set; }

    public string? Notes { get; set; }

    public required Variant Variant { get; set; }
    public ICollection<ConfigurationAvionics> ConfigurationAvionics { get; set; } = [];
}