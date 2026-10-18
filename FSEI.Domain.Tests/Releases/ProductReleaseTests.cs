using FSEI.Domain.Products;
using FSEI.Domain.Releases;
using NUnit.Framework;

namespace FSEI.Domain.Tests.Releases;

[TestFixture]
public class ProductReleaseTests
{
    [Test]
    public void Product_CanHaveMultipleReleases()
    {
        // Arrange
        var product = new Product
        {
            Name = "BetterPushback",
            ProductType = new ProductType
            {
                Code = "PLUGIN",
                Name = "Plugin"
            }
        };

        var stableRelease = new ProductRelease
        {
            Product = product,
            Version = "0.52",
            ReleaseChannel = ReleaseChannel.Stable
        };

        var betaRelease = new ProductRelease
        {
            Product = product,
            Version = "0.53-beta",
            ReleaseChannel = ReleaseChannel.Beta
        };

        // Act
        product.Releases.Add(stableRelease);
        product.Releases.Add(betaRelease);

        // Assert
        Assert.That(product.Releases, Has.Count.EqualTo(2));
        Assert.That(product.Releases, Does.Contain(stableRelease));
        Assert.That(product.Releases, Does.Contain(betaRelease));
    }
    
    [Test]
    public void ProductRelease_PreservesVersionInformation()
    {
        // Arrange
        var product = new Product
        {
            Name = "X-Plane Add-on",
            ProductType = new ProductType
            {
                Code = "PLUGIN",
                Name = "Plugin"
            }
        };

        var release = new ProductRelease
        {
            Product = product,
            Version = "2.4.1-beta",
            MajorVersion = 2,
            MinorVersion = 4,
            PatchVersion = 1,
            ReleaseChannel = ReleaseChannel.Beta
        };

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(release.Version, Is.EqualTo("2.4.1-beta"));
            Assert.That(release.MajorVersion, Is.EqualTo(2));
            Assert.That(release.MinorVersion, Is.EqualTo(4));
            Assert.That(release.PatchVersion, Is.EqualTo(1));
            Assert.That(release.ReleaseChannel, Is.EqualTo(ReleaseChannel.Beta));
        });
    }
    [Test]
    public void ProductRelease_StableAndBetaAreDistinctReleases()
    {
        // Arrange
        var product = new Product
        {
            Name = "BetterPushback",
            ProductType = new ProductType
            {
                Code = "PLUGIN",
                Name = "Plugin"
            }
        };

        var beta = new ProductRelease
        {
            Product = product,
            Version = "1.2.0-beta",
            MajorVersion = 1,
            MinorVersion = 2,
            PatchVersion = 0,
            ReleaseChannel = ReleaseChannel.Beta
        };

        var stable = new ProductRelease
        {
            Product = product,
            Version = "1.2.0",
            MajorVersion = 1,
            MinorVersion = 2,
            PatchVersion = 0,
            ReleaseChannel = ReleaseChannel.Stable
        };

        // Act
        product.Releases.Add(beta);
        product.Releases.Add(stable);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(product.Releases, Has.Count.EqualTo(2));
            Assert.That(beta.ReleaseChannel, Is.Not.EqualTo(stable.ReleaseChannel));
            Assert.That(beta.Version, Is.Not.EqualTo(stable.Version));
        });
    }
    
    [Test]
    public void ProductRelease_NegativeMajorVersion_ThrowsException()
    {
        var product = new Product
        {
            Name = "Test Plugin",
            ProductType = new ProductType
            {
                Code = "PLUGIN",
                Name = "Plugin"
            }
        };

        var release = new ProductRelease
        {
            Product = product,
            Version = "1.0.0"
        };

        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            release.MajorVersion = -1;
        });
    }
    
    [Test]
    public void ProductRelease_NegativeMinorVersion_ThrowsException()
    {
        // Arrange
        var product = new Product
        {
            Name = "Test Plugin",
            ProductType = new ProductType
            {
                Code = "PLUGIN",
                Name = "Plugin"
            }
        };

        var release = new ProductRelease
        {
            Product = product,
            Version = "1.0.0"
        };

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            release.MinorVersion = -1;
        });
    }
    
    [Test]
    public void ProductRelease_NegativePatchVersion_ThrowsException()
    {
        // Arrange
        var product = new Product
        {
            Name = "Test Plugin",
            ProductType = new ProductType
            {
                Code = "PLUGIN",
                Name = "Plugin"
            }
        };

        var release = new ProductRelease
        {
            Product = product,
            Version = "1.0.0"
        };

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            release.PatchVersion = -1;
        });
    }
    
    [Test]
    public void ProductRelease_DefaultStatus_IsUnknown()
    {
        // Arrange
        var product = new Product
        {
            Name = "BetterPushback",
            ProductType = new ProductType
            {
                Code = "PLUGIN",
                Name = "Plugin"
            }
        };

        // Act
        var release = new ProductRelease
        {
            Product = product,
            Version = "1.0.0",
            ReleaseChannel = ReleaseChannel.Stable
        };

        // Assert
        Assert.That(release.ReleaseStatus,
            Is.EqualTo(ReleaseStatus.Unknown));
    }
    
    [Test]
    public void ProductRelease_CanBeMarkedWithdrawn()
    {
        var product = new Product
        {
            Name = "Test Aircraft",
            ProductType = new ProductType
            {
                Code = "AIRCRAFT",
                Name = "Aircraft"
            }
        };

        var release = new ProductRelease
        {
            Product = product,
            Version = "2.1.0",
            ReleaseChannel = ReleaseChannel.Stable,
            ReleaseStatus = ReleaseStatus.Current
        };

        // Simulate a release being withdrawn.
        release.ReleaseStatus = ReleaseStatus.Withdrawn;

        Assert.That(release.ReleaseStatus,
            Is.EqualTo(ReleaseStatus.Withdrawn));
    }
    [Test]
    public void ProductRelease_CanBeMarkedSuperseded()
    {
        var product = new Product
        {
            Name = "BetterPushback",
            ProductType = new ProductType
            {
                Code = "PLUGIN",
                Name = "Plugin"
            }
        };

        var release = new ProductRelease
        {
            Product = product,
            Version = "1.0.0",
            ReleaseChannel = ReleaseChannel.Stable,
            ReleaseStatus = ReleaseStatus.Current
        };

        // A newer release replaces this version.
        release.ReleaseStatus = ReleaseStatus.Superseded;

        Assert.That(release.ReleaseStatus,
            Is.EqualTo(ReleaseStatus.Superseded));
    }
    
    [Test]
    public void ProductRelease_DefaultChannel_IsUnknown()
    {
        var product = new Product
        {
            Name = "Test Plugin",
            ProductType = new ProductType
            {
                Code = "PLUGIN",
                Name = "Plugin"
            }
        };

        var release = new ProductRelease
        {
            Product = product,
            Version = "1.0.0"
        };

        Assert.That(release.ReleaseChannel,
            Is.EqualTo(ReleaseChannel.Unknown));
    }
}