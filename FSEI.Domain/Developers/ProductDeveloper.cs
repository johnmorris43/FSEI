using FSEI.Domain.Developers;

namespace FSEI.Domain.Products;

public class ProductDeveloper
{
    public int ProductDeveloperId { get; set; }

    public int ProductId { get; set; }

    public int DeveloperId { get; set; }

    public string? Role { get; set; }

    public required Product Product { get; set; }

    public required Developer Developer { get; set; }
}