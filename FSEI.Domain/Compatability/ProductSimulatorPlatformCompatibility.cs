using FSEI.Domain.Platforms;

namespace FSEI.Domain.Compatibility;

public class ProductSimulatorPlatformCompatibility
{
    public int ProductSimulatorPlatformCompatibilityId { get; set; }

    public int ProductSimulatorCompatibilityId { get; set; }

    public int OperatingSystemId { get; set; }

    public int? ProcessorArchitectureId { get; set; }

    public ExecutionMethod ExecutionMethod { get; set; } = ExecutionMethod.Unknown;

    public int CompatibilityStatusId { get; set; }

    public string? Notes { get; set; }

    public required ProductSimulatorCompatibility
        ProductSimulatorCompatibility { get; set; }

    public required FSEI.Domain.Platforms.OperatingSystem OperatingSystem { get; set; }

    public ProcessorArchitecture? ProcessorArchitecture { get; set; }

    public required CompatibilityStatus CompatibilityStatus { get; set; }
    
    public void ValidatePlatform()
    {
        if (OperatingSystemId <= 0)
        {
            throw new InvalidOperationException(
                "A platform compatibility record must specify an operating system.");
        }

        if (OperatingSystem is not null &&
            OperatingSystem.OperatingSystemId != OperatingSystemId)
        {
            throw new InvalidOperationException(
                "The operating system reference does not match the selected operating system.");
        }
    }
}