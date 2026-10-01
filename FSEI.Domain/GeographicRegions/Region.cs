namespace FSEI.Domain.GeographicRegions;

public class Region
{
    public int RegionId { get; set; }

    public int? ParentRegionId { get; set; }

    public required string Code { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public Region? ParentRegion { get; set; }

    public ICollection<Region> ChildRegions { get; set; } = [];
    
    public ICollection<Market> Markets { get; set; } = [];
}