using FSEI.Domain.Simulator;

namespace FSEI.Domain.Platforms;

public class SimulatorOperatingSystemCompatibility
{
    public int SimulatorOperatingSystemCompatibilityId { get; set; }
    public int SimulatorId { get; set; }
    public int OperatingSystemId { get; set; }

    public string? Notes { get; set; }

    public required FSEI.Domain.Simulator.Simulator Simulator { get; set; }
    public required OperatingSystem OperatingSystem { get; set; }
}