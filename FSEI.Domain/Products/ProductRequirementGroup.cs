namespace FSEI.Domain.Products;

public class ProductRequirementGroup
{
    public int ProductRequirementGroupId { get; set; }

    public int ProductId { get; set; }

    public string? Description { get; set; }

    public required Product Product { get; set; }
    
    public bool IsSatisfiedBy(IEnumerable<int> ownedProductIds)
    {
        return Requirements.Any(
            requirement => ownedProductIds.Contains(requirement.RequiredProductId));
    }
    
    public void Validate()
    {
        if (Requirements.Count == 0)
        {
            throw new InvalidOperationException(
                "A product requirement group must contain at least one requirement.");
        }
    }

    public ICollection<ProductRequirement> Requirements { get; set; } = [];
}