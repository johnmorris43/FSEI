using FSEI.Domain.Products;

namespace FSEI.Domain.Developers;

public class Developer
{
    public int DeveloperId { get; set; }

    public required string Name { get; set; }

    public string? WebsiteUrl { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;
    
    public ICollection<ProductDeveloper> ProductDevelopers { get; set; } = [];
}