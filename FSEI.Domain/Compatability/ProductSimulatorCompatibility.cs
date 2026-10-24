using FSEI.Domain.Products;
using FSEI.Domain.Releases;
using FSEI.Domain.Simulators;
using FSEI.Domain.Platforms;

namespace FSEI.Domain.Compatibility;

public class ProductSimulatorCompatibility
{
    public int ProductSimulatorCompatibilityId { get; set; }

    public int ProductId { get; set; }
    public int SimulatorId { get; set; }
    public int CompatibilityStatusId { get; set; }

    public int? SupportedFromProductReleaseId { get; set; }
    public int? SupportedThroughProductReleaseId { get; set; }

    public int? SupportedFromSimulatorReleaseId { get; set; }
    public int? SupportedThroughSimulatorReleaseId { get; set; }

    public string? Notes { get; set; }

    public required Product Product { get; set; }
    public required Simulator Simulator { get; set; }
    public required CompatibilityStatus CompatibilityStatus { get; set; }

    public ProductRelease? SupportedFromProductRelease { get; set; }
    public ProductRelease? SupportedThroughProductRelease { get; set; }

    public SimulatorRelease? SupportedFromSimulatorRelease { get; set; }
    public SimulatorRelease? SupportedThroughSimulatorRelease { get; set; }
    
    public void ValidateReleaseOwnership()
    {
        if (SupportedFromProductRelease is not null &&
            SupportedFromProductRelease.ProductId != ProductId)
        {
            throw new InvalidOperationException(
                "Starting product release belongs to a different product.");
        }

        if (SupportedThroughProductRelease is not null &&
            SupportedThroughProductRelease.ProductId != ProductId)
        {
            throw new InvalidOperationException(
                "Ending product release belongs to a different product.");
        }

        if (SupportedFromSimulatorRelease is not null &&
            SupportedFromSimulatorRelease.SimulatorId != SimulatorId)
        {
            throw new InvalidOperationException(
                "Starting simulator release belongs to a different simulator.");
        }

        if (SupportedThroughSimulatorRelease is not null &&
            SupportedThroughSimulatorRelease.SimulatorId != SimulatorId)
        {
            throw new InvalidOperationException(
                "Ending simulator release belongs to a different simulator.");
        }
    }
}

