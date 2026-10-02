namespace FSEI.Domain.Stores;

public class BillingInterval
{
    public int BillingIntervalId { get; set; }
    public required string Code { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}