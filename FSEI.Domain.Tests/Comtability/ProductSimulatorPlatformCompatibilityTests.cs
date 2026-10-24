using FSEI.Domain.Compatibility;
using NUnit.Framework;

namespace FSEI.Domain.Tests.Compatibility;

[TestFixture]
public class ProductSimulatorPlatformCompatibilityTests
{
    [Test]
    public void ValidatePlatform_RejectsInvalidOperatingSystemId()
    {
        var compatibility = new ProductSimulatorPlatformCompatibility
        {
            OperatingSystemId = 0,
            ProductSimulatorCompatibility = null!,
            OperatingSystem = null!,
            CompatibilityStatus = null!
        };

        Assert.Throws<InvalidOperationException>(
            () => compatibility.ValidatePlatform());
    }
    
    [Test]
    public void ValidatePlatform_RejectsMismatchedOperatingSystem()
    {
        var operatingSystem = new FSEI.Domain.Platforms.OperatingSystem
        {
            OperatingSystemId = 2,
            Name = "Parrot OS",
            Family = FSEI.Domain.Platforms.OperatingSystemFamily.Linux
        };

        var compatibility = new ProductSimulatorPlatformCompatibility
        {
            OperatingSystemId = 1,
            OperatingSystem = operatingSystem,
            ProductSimulatorCompatibility = null!,
            CompatibilityStatus = null!
        };

        Assert.Throws<InvalidOperationException>(
            () => compatibility.ValidatePlatform());
    }
    
    [Test]
    public void ValidatePlatform_AcceptsMatchingOperatingSystem()
    {
        var operatingSystem = new FSEI.Domain.Platforms.OperatingSystem
        {
            OperatingSystemId = 2,
            Name = "Parrot OS",
            Family = FSEI.Domain.Platforms.OperatingSystemFamily.Linux
        };

        var compatibility = new ProductSimulatorPlatformCompatibility
        {
            OperatingSystemId = 2,
            OperatingSystem = operatingSystem,
            ProductSimulatorCompatibility = null!,
            CompatibilityStatus = null!
        };

        Assert.DoesNotThrow(
            () => compatibility.ValidatePlatform());
    }
    
    [Test]
    public void ExecutionMethod_DoesNotChangeOperatingSystemFamily()
    {
        var operatingSystem = new FSEI.Domain.Platforms.OperatingSystem
        {
            OperatingSystemId = 2,
            Name = "Parrot OS",
            Family = FSEI.Domain.Platforms.OperatingSystemFamily.Linux
        };

        var compatibility = new ProductSimulatorPlatformCompatibility
        {
            OperatingSystemId = 2,
            OperatingSystem = operatingSystem,
            ProductSimulatorCompatibility = null!,
            CompatibilityStatus = null!
        };

        var executionMethods = new[]
        {
            FSEI.Domain.Platforms.ExecutionMethod.Native,
            FSEI.Domain.Platforms.ExecutionMethod.Wine,
            FSEI.Domain.Platforms.ExecutionMethod.Proton
        };

        foreach (var method in executionMethods)
        {
            compatibility.ExecutionMethod = method;

            Assert.That(compatibility.ExecutionMethod, Is.EqualTo(method));

            Assert.That(
                compatibility.OperatingSystem.Family,
                Is.EqualTo(FSEI.Domain.Platforms.OperatingSystemFamily.Linux));

            Assert.DoesNotThrow(
                () => compatibility.ValidatePlatform());
        }

        Assert.That(
            executionMethods.Distinct().Count(),
            Is.EqualTo(3));
    }
}