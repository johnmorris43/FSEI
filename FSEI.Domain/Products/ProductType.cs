namespace FSEI.Domain.Products;

public class ProductType
{
    public int ProductTypeId { get; set; }

    public required string Code { get; set; }
    public required string Name { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<Product> Products { get; set; } = [];
}