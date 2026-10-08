using FSEI.Domain.Avionics;

namespace FSEI.Domain.Aircraft;



public class AircraftConfiguration
{
    public int AircraftConfigurationId { get; set; }

    public int AircraftVariantId { get; set; }

    public required string Name { get; set; }

    public string? Notes { get; set; }

    public required AircraftVariant AircraftVariant { get; set; }
    public ICollection<ConfigurationAvionics> ConfigurationAvionics { get; set; } = [];
}