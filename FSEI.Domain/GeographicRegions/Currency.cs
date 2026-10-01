namespace FSEI.Domain.GeographicRegions;

public class Currency
{
    public int CurrencyId { get; set; }

    public required string Code { get; set; }

    public required string Name { get; set; }

    public string? Symbol { get; set; }

    public string? Notes { get; set; }

    public bool IsActive { get; set; } = true;
    
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Code))
        {
            throw new InvalidOperationException(
                "A currency must have a code.");
        }

        if (Code.Length != 3)
        {
            throw new InvalidOperationException(
                "A currency code must contain exactly three characters.");
        }
    }
}