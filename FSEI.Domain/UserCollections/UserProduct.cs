using FSEI.Domain.Products;

namespace FSEI.Domain.UserCollections;

public class UserProduct
{
    public int UserProductId { get; set; }

    public int? ProductId { get; set; }

    public required string Name { get; set; }

    public string? Notes { get; set; }

    public Product? Product { get; set; }
    
    
    public void Validate()
    {
        if (ProductId.HasValue)
        {
            if (Product != null && Product.ProductId != ProductId.Value)
            {
                throw new InvalidOperationException(
                    "ProductId does not match the referenced catalog product.");
            }
        }
        else if (Product != null)
        {
            throw new InvalidOperationException(
                "A catalog product cannot be referenced without a ProductId.");
        }
    }
}