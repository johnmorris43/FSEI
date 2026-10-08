using FSEI.Domain.Platforms;
using FSEI.Domain.Simulators;

namespace FSEI.Domain.Tests;

public class SimulatorOperatingSystemCompatibilityTests
{
    [Test]
    public void ValidateReleaseBoundaries_WhenReleasesBelongToSimulator_DoesNotThrow()
    {
        var compatibility = CreateCompatibility();

        compatibility.SupportedFromRelease = CreateRelease(1, "11.50");
        compatibility.SupportedThroughRelease = CreateRelease(1, "11.55");

        Assert.DoesNotThrow(
            compatibility.ValidateReleaseBoundaries);
    }

    [Test]
    public void ValidateReleaseBoundaries_WhenFromReleaseBelongsToDifferentSimulator_ThrowsException()
    {
        var compatibility = CreateCompatibility();

        compatibility.SupportedFromRelease = CreateRelease(2, "12.1.4");

        Assert.Throws<InvalidOperationException>(
            compatibility.ValidateReleaseBoundaries);
    }

    [Test]
    public void ValidateReleaseBoundaries_WhenThroughReleaseBelongsToDifferentSimulator_ThrowsException()
    {
        var compatibility = CreateCompatibility();

        compatibility.SupportedThroughRelease = CreateRelease(2, "12.1.4");

        Assert.Throws<InvalidOperationException>(
            compatibility.ValidateReleaseBoundaries);
    }
    
    [Test]
    public void ValidateReleaseBoundaries_WhenNoReleaseBoundariesAreSet_DoesNotThrow()
    {
        var compatibility = CreateCompatibility();

        Assert.DoesNotThrow(
            compatibility.ValidateReleaseBoundaries);
    }
    
    private static SimulatorOperatingSystemCompatibility CreateCompatibility()
    {
        return new SimulatorOperatingSystemCompatibility
        {
            SimulatorId = 1,

            Simulator = null!,
            OperatingSystem = null!,
            CompatibilityStatus = null!
        };
    }

    private static SimulatorRelease CreateRelease(
        int simulatorId,
        string version)
    {
        return new SimulatorRelease
        {
            SimulatorId = simulatorId,
            Version = version,
            Simulator = null!
        };
    }
}