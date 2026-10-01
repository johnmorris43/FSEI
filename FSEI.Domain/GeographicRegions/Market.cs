namespace FSEI.Domain.GeographicRegions;

public class Market
{
    public int MarketId { get; set; }

    public int RegionId { get; set; }

    public required string Code { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public required Region Region { get; set; }
    
    public void Validate()
    {
        if (RegionId != Region.RegionId)
        {
            throw new InvalidOperationException(
                "A market's RegionId must match its Region.");
        }
    }
}