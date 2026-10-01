namespace FSEI.Domain.Stores;

public class Storefront
{
    public int StorefrontId { get; set; }

    public required string Name { get; set; }

    public string? Url { get; set; }

    public string? Notes { get; set; }

    public bool IsActive { get; set; } = true;
    
    public ICollection<CommercialOffer> CommercialOffers { get; set; } = [];
}