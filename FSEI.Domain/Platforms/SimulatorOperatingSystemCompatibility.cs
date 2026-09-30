using FSEI.Domain.Simulators;

namespace FSEI.Domain.Platforms;

public class SimulatorOperatingSystemCompatibility
{
    public int SimulatorOperatingSystemCompatibilityId { get; set; }
    public int SimulatorId { get; set; }
    public int? ProcessorArchitectureId { get; set; }
    public int? SupportedFromReleaseId { get; set; }
    public int? SupportedThroughReleaseId { get; set; }


    public int OperatingSystemId { get; set; }
    public int CompatibilityStatusId { get; set; }

    public string? Notes { get; set; }

    public SimulatorRelease? SupportedFromRelease { get; set; }
    public SimulatorRelease? SupportedThroughRelease { get; set; }
    public  ProcessorArchitecture? ProcessorArchitecture { get; set; }
    public required CompatibilityStatus CompatibilityStatus { get; set; }
    public required FSEI.Domain.Simulators.Simulator Simulator { get; set; }
    public required OperatingSystem OperatingSystem { get; set; }
    
    
    
    public void ValidateReleaseBoundaries()
    {
        if (SupportedFromRelease is not null &&
            SupportedFromRelease.SimulatorId != SimulatorId)
        {
            throw new InvalidOperationException(
                "The supported-from release must belong to the compatibility record's simulator.");
        }

        if (SupportedThroughRelease is not null &&
            SupportedThroughRelease.SimulatorId != SimulatorId)
        {
            throw new InvalidOperationException(
                "The supported-through release must belong to the compatibility record's simulator.");
        }
    }
}