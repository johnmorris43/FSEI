namespace FSEI.Domain.Products;

public class ProductRequirement
{
    public int ProductRequirementId { get; set; }

    public int ProductId { get; set; }
    public int RequiredProductId { get; set; }

    public string? Notes { get; set; }

    public required Product Product { get; set; }
    public required Product RequiredProduct { get; set; }

    public void Validate()
    {
        if (ProductId == RequiredProductId)
        {
            throw new InvalidOperationException(
                "A product cannot require itself.");
        }
    }
}