namespace FSEI.Domain.Platforms;

public class OperatingSystem
{
    public int OperatingSystemId { get; set; }
    public required string Name { get; set; }
    public string? Version { get; set; }
    public string? Notes { get; set; }

    public ICollection<SimulatorOperatingSystemCompatibility> SimulatorOperatingSystemCompatibilities { get; set; } =
        [];
}