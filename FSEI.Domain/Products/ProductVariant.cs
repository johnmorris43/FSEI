using FSEI.Domain.Aircraft;

namespace FSEI.Domain.Products;

public class ProductVariant
{
    public int ProductVariantId { get; set; }

    public int ProductId { get; set; }
    public int AircraftVariantId { get; set; }

    public string? Notes { get; set; }

    public required Product Product { get; set; }
    public required AircraftVariant AircraftVariant { get; set; }
}