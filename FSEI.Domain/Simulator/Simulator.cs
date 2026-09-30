namespace FSEI.Domain.Simulator;
using FSEI.Domain.Platforms;

public class Simulator
{
    public int SimulatorId { get; set; }
    public int SimulatorFamilyId { get; set; }
    public int SimulatorStatusId { get; set; }
    
    public required string Name { get; set; }
    public string? Version { get; set; }
    public string? Notes { get; set; }
    
    public required SimulatorStatus SimulatorStatus { get; set; }
    public required SimulatorFamily SimulatorFamily { get; set; }
    
    public ICollection<SimulatorOperatingSystemCompatibility> SimulatorOperatingSystemCompatibilities { get; set; } =
        [];
}