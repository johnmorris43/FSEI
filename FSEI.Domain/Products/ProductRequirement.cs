namespace FSEI.Domain.Products;

public class ProductRequirement
{
    public int ProductRequirementId { get; set; }

    public int ProductRequirementGroupId { get; set; }
    public int RequiredProductId { get; set; }

    public string? Notes { get; set; }

    public required ProductRequirementGroup ProductRequirementGroup { get; set; }
    public required Product RequiredProduct { get; set; }

    public void Validate()
    {
        if (ProductRequirementGroup.ProductId == RequiredProductId)
        {
            throw new InvalidOperationException(
                "A product cannot require itself.");
        }
    }
}