namespace FSEI.Domain.Platforms;

public class ProcessorArchitecture
{
    public int ProcessorArchitectureId { get; set; }
    public required string Code { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<SimulatorOperatingSystemCompatibility>
        SimulatorOperatingSystemCompatibilities { get; set; } = [];
}