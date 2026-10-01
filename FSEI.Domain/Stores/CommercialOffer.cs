using FSEI.Domain.Products;

namespace FSEI.Domain.Stores;

public class CommercialOffer
{
    public int CommercialOfferId { get; set; }

    public int ProductId { get; set; }

    public int StorefrontId { get; set; }

    public string? Url { get; set; }

    public string? Notes { get; set; }

    public bool IsActive { get; set; } = true;

    public required Product Product { get; set; }

    public required Storefront Storefront { get; set; }
    
    public ICollection<OfferOption> OfferOptions { get; set; } = [];
}