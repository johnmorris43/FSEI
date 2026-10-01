namespace FSEI.Domain.Products;
using FSEI.Domain.Stores;
public class Product
{
    public int ProductId { get; set; }

    public int ProductTypeId { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }
    public string? Notes { get; set; }

    public bool IsActive { get; set; } = true;

    public required ProductType ProductType { get; set; }

    public ICollection<ProductVariant> ProductVariants { get; set; } = [];
    
    public ICollection<ProductRequirementGroup> RequirementGroups { get; set; } = [];
    
    public ICollection<CommercialOffer> CommercialOffers { get; set; } = [];
}