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
    
    [Test]
    public void ProductRelease_CanContainMultipleChanges()
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
        // Arrange
        var release = new ProductRelease
        {
            Product = product,
            Version = "1.15",
            ReleaseChannel = ReleaseChannel.Stable
        };

        var feature = new ReleaseChange
        {
            ChangeType = ReleaseChangeType.Feature,
            Description = "Added automatic disconnect."
        };

        var bugFix = new ReleaseChange
        {
            ChangeType = ReleaseChangeType.BugFix,
            Description = "Fixed airport cache handling."
        };

        // Act
        release.Changes.Add(feature);
        release.Changes.Add(bugFix);

        // Assert
        Assert.That(release.Changes, Has.Count.EqualTo(2));

        Assert.That(
            release.Changes.Select(c => c.ChangeType),
            Is.EquivalentTo(new[]
            {
                ReleaseChangeType.Feature,
                ReleaseChangeType.BugFix
            }));
    }
    [Test]
    public void ReleaseChange_ReferencesCorrectProductRelease()
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

        var release = new ProductRelease
        {
            Product = product,
            Version = "1.15",
            ReleaseChannel = ReleaseChannel.Stable
        };

        var change = new ReleaseChange
        {
            ProductRelease = release,
            ChangeType = ReleaseChangeType.BugFix,
            Description = "Fixed airport cache handling."
        };

        // Act
        release.Changes.Add(change);

        // Assert
        Assert.That(change.ProductRelease, Is.SameAs(release));
        Assert.That(release.Changes, Does.Contain(change));
    }
    
    [Test]
    public void ReleaseChange_DefaultChangeType_IsUnknown()
    {
        // Arrange
        var change = new ReleaseChange();

        // Assert
        Assert.That(
            change.ChangeType,
            Is.EqualTo(ReleaseChangeType.Unknown));
    }
    
    [Test]
    public void ReleaseChange_PreservesOriginalDescription()
    {
        // Arrange
        const string originalDescription =
            "Fixed airport cache handling in X-Plane 12.";

        var change = new ReleaseChange
        {
            ChangeType = ReleaseChangeType.Unknown,
            Description = originalDescription
        };

        // Assert
        Assert.That(
            change.Description,
            Is.EqualTo(originalDescription));
    }
    [Test]
    public void ProductRelease_CanStorePreReleaseNumber()
    {
        // Arrange
        var product = new Product
        {
            Name = "X-Plane 12",
            ProductType = new ProductType
            {
                Code = "SIMULATOR",
                Name = "Simulator"
            }
        };

        // Act
        var release = new ProductRelease
        {
            Product = product,
            Version = "12.4.4-B2",
            ReleaseChannel = ReleaseChannel.Beta,
            PreReleaseNumber = 2
        };

        // Assert
        Assert.That(release.PreReleaseNumber, Is.EqualTo(2));
        Assert.That(release.ReleaseChannel, Is.EqualTo(ReleaseChannel.Beta));
        Assert.That(release.Version, Is.EqualTo("12.4.4-B2"));
    }
    
    [Test]
    public void ProductRelease_NegativePreReleaseNumber_ThrowsException()
    {
        // Arrange
        var product = new Product
        {
            Name = "X-Plane 12",
            ProductType = new ProductType
            {
                Code = "SIMULATOR",
                Name = "Simulator"
            }
        };

        var release = new ProductRelease
        {
            Product = product,
            Version = "12.4.4-B2"
        };

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            release.PreReleaseNumber = -1;
        });
    }
    
    [Test]
    public void ProductRelease_DefaultPreReleaseNumber_IsNull()
    {
        // Arrange
        var product = new Product
        {
            Name = "X-Plane 12",
            ProductType = new ProductType
            {
                Code = "SIMULATOR",
                Name = "Simulator"
            }
        };

        // Act
        var release = new ProductRelease
        {
            Product = product,
            Version = "12.4.4",
            ReleaseChannel = ReleaseChannel.Stable
        };

        // Assert
        Assert.That(release.PreReleaseNumber, Is.Null);
    }
    
    [Test]
    public void ProductRelease_DifferentBetaIterations_AreDistinct()
    {
        // Arrange
        var product = new Product
        {
            Name = "X-Plane 12",
            ProductType = new ProductType
            {
                Code = "SIMULATOR",
                Name = "Simulator"
            }
        };

        var beta1 = new ProductRelease
        {
            Product = product,
            Version = "12.4.4-B1",
            MajorVersion = 12,
            MinorVersion = 4,
            PatchVersion = 4,
            ReleaseChannel = ReleaseChannel.Beta,
            PreReleaseNumber = 1
        };

        var beta2 = new ProductRelease
        {
            Product = product,
            Version = "12.4.4-B2",
            MajorVersion = 12,
            MinorVersion = 4,
            PatchVersion = 4,
            ReleaseChannel = ReleaseChannel.Beta,
            PreReleaseNumber = 2
        };

        // Act
        product.Releases.Add(beta1);
        product.Releases.Add(beta2);

        // Assert
        Assert.That(product.Releases, Has.Count.EqualTo(2));

        Assert.That(beta1.PreReleaseNumber, Is.EqualTo(1));
        Assert.That(beta2.PreReleaseNumber, Is.EqualTo(2));

        Assert.That(beta1.Version, Is.Not.EqualTo(beta2.Version));
    }
    
    [Test]
    public void ProductRelease_BuildNumberAndPreReleaseNumber_AreIndependent()
    {
        // Arrange
        var product = new Product
        {
            Name = "X-Plane 12",
            ProductType = new ProductType
            {
                Code = "SIMULATOR",
                Name = "Simulator"
            }
        };

        // Act
        var release = new ProductRelease
        {
            Product = product,
            Version = "12.4.4-B2",
            ReleaseChannel = ReleaseChannel.Beta,
            PreReleaseNumber = 2,
            BuildNumber = "20261019"
        };

        // Assert
        Assert.That(release.PreReleaseNumber, Is.EqualTo(2));
        Assert.That(release.BuildNumber, Is.EqualTo("20261019"));
    }
}