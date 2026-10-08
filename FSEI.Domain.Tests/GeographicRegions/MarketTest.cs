using FSEI.Domain.GeographicRegions;

namespace FSEI.Domain.Tests;

public class MarketTests
{
    [Test]
    public void Validate_WhenRegionMatches_DoesNotThrow()
    {
        var region = CreateRegion(1);

        var market = new Market
        {
            MarketId = 1,
            RegionId = 1,
            Code = "DE",
            Name = "German Market",
            Region = region
        };

        Assert.DoesNotThrow(() => market.Validate());
    }

    [Test]
    public void Validate_WhenRegionDoesNotMatch_ThrowsInvalidOperationException()
    {
        var region = CreateRegion(1);

        var market = new Market
        {
            MarketId = 1,
            RegionId = 2,
            Code = "DE",
            Name = "German Market",
            Region = region
        };

        Assert.Throws<InvalidOperationException>(() => market.Validate());
    }

    private static Region CreateRegion(int regionId)
    {
        return new Region
        {
            RegionId = regionId,
            Code = "DE",
            Name = "Germany"
        };
    }
}