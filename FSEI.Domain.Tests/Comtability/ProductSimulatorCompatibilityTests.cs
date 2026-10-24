using FSEI.Domain.Compatibility;
using FSEI.Domain.Platforms;
using FSEI.Domain.Products;
using FSEI.Domain.Releases;
using FSEI.Domain.Simulators;
using NUnit.Framework;

namespace FSEI.Domain.Tests.Compatibility;

[TestFixture]
public class ProductSimulatorCompatibilityTests
{
    [Test]
    public void ValidateReleaseOwnership_RejectsReleaseFromDifferentProduct()
    {
        var product = new Product
        {
            ProductId = 1,
            Name = "Test Product",
            ProductType = null!
        };

        var simulator = new Simulator
        {
            SimulatorId = 10,
            Name = "Test Simulator",
            SimulatorFamily = null!,
            SimulatorStatus = null!
        };

        var wrongRelease = new ProductRelease
        {
            ProductReleaseId = 100,
            ProductId = 2,
            Version = "1.0",
            Product = product
        };

        var compatibility = new ProductSimulatorCompatibility
        {
            ProductId = 1,
            SimulatorId = 10,
            CompatibilityStatusId = 1,
            Product = product,
            Simulator = simulator,
            CompatibilityStatus = null!,
            SupportedFromProductRelease = wrongRelease
        };

        Assert.Throws<InvalidOperationException>(
            () => compatibility.ValidateReleaseOwnership());
    }
    
    [Test]
    public void ValidateReleaseOwnership_RejectsEndingReleaseFromDifferentProduct()
    {
        var product = new Product
        {
            ProductId = 1,
            Name = "Test Product",
            ProductType = null!
        };

        var simulator = new Simulator
        {
            SimulatorId = 10,
            Name = "Test Simulator",
            SimulatorFamily = null!,
            SimulatorStatus = null!
        };

        var wrongRelease = new ProductRelease
        {
            ProductReleaseId = 101,
            ProductId = 2,
            Version = "2.0",
            Product = product
        };

        var compatibility = new ProductSimulatorCompatibility
        {
            ProductId = 1,
            SimulatorId = 10,
            CompatibilityStatusId = 1,
            Product = product,
            Simulator = simulator,
            CompatibilityStatus = null!,
            SupportedThroughProductRelease = wrongRelease
        };

        Assert.Throws<InvalidOperationException>(
            () => compatibility.ValidateReleaseOwnership());
    }
    
    [Test]
    public void ValidateReleaseOwnership_RejectsStartingReleaseFromDifferentSimulator()
    {
        var product = new Product
        {
            ProductId = 1,
            Name = "Test Product",
            ProductType = null!
        };

        var simulator = new Simulator
        {
            SimulatorId = 10,
            Name = "Test Simulator",
            SimulatorFamily = null!,
            SimulatorStatus = null!
        };

        var wrongRelease = new SimulatorRelease
        {
            SimulatorReleaseId = 200,
            SimulatorId = 20,
            Version = "12.4",
            Simulator = simulator
        };

        var compatibility = new ProductSimulatorCompatibility
        {
            ProductId = 1,
            SimulatorId = 10,
            CompatibilityStatusId = 1,
            Product = product,
            Simulator = simulator,
            CompatibilityStatus = null!,
            SupportedFromSimulatorRelease = wrongRelease
        };

        Assert.Throws<InvalidOperationException>(
            () => compatibility.ValidateReleaseOwnership());
    }
    
    [Test]
    public void ValidateReleaseOwnership_RejectsEndingReleaseFromDifferentSimulator()
    {
        var product = new Product
        {
            ProductId = 1,
            Name = "Test Product",
            ProductType = null!
        };

        var simulator = new Simulator
        {
            SimulatorId = 10,
            Name = "Test Simulator",
            SimulatorFamily = null!,
            SimulatorStatus = null!
        };

        var wrongRelease = new SimulatorRelease
        {
            SimulatorReleaseId = 201,
            SimulatorId = 20,
            Version = "12.5",
            Simulator = simulator
        };

        var compatibility = new ProductSimulatorCompatibility
        {
            ProductId = 1,
            SimulatorId = 10,
            CompatibilityStatusId = 1,
            Product = product,
            Simulator = simulator,
            CompatibilityStatus = null!,
            SupportedThroughSimulatorRelease = wrongRelease
        };

        Assert.Throws<InvalidOperationException>(
            () => compatibility.ValidateReleaseOwnership());
    }
    
    [Test]
    public void ValidateReleaseOwnership_AcceptsMatchingReleases()
    {
        var product = new Product
        {
            ProductId = 1,
            Name = "Test Product",
            ProductType = null!
        };

        var simulator = new Simulator
        {
            SimulatorId = 10,
            Name = "Test Simulator",
            SimulatorFamily = null!,
            SimulatorStatus = null!
        };

        var productRelease = new ProductRelease
        {
            ProductReleaseId = 100,
            ProductId = 1,
            Version = "1.0",
            Product = product
        };

        var simulatorRelease = new SimulatorRelease
        {
            SimulatorReleaseId = 200,
            SimulatorId = 10,
            Version = "12.4",
            Simulator = simulator
        };

        var compatibility = new ProductSimulatorCompatibility
        {
            ProductId = 1,
            SimulatorId = 10,
            CompatibilityStatusId = 1,
            Product = product,
            Simulator = simulator,
            CompatibilityStatus = null!,
            SupportedFromProductRelease = productRelease,
            SupportedThroughProductRelease = productRelease,
            SupportedFromSimulatorRelease = simulatorRelease,
            SupportedThroughSimulatorRelease = simulatorRelease
        };

        Assert.DoesNotThrow(
            () => compatibility.ValidateReleaseOwnership());
    }
    
    [Test]
    public void ValidateReleaseOwnership_AcceptsNoReleaseBoundaries()
    {
        var product = new Product
        {
            ProductId = 1,
            Name = "Test Product",
            ProductType = null!
        };

        var simulator = new Simulator
        {
            SimulatorId = 10,
            Name = "Test Simulator",
            SimulatorFamily = null!,
            SimulatorStatus = null!
        };

        var compatibility = new ProductSimulatorCompatibility
        {
            ProductId = 1,
            SimulatorId = 10,
            CompatibilityStatusId = 1,
            Product = product,
            Simulator = simulator,
            CompatibilityStatus = null!
        };

        Assert.DoesNotThrow(
            () => compatibility.ValidateReleaseOwnership());
    }
    
    
}