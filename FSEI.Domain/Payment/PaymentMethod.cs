namespace FSEI.Domain.Payment;

public class PaymentMethod
{
    public int PaymentMethodId { get; set; }

    
    public required string Code { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;
}