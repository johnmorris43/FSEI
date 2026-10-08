using FSEI.Domain.Payment;

namespace FSEI.Domain.Stores;

public class StorefrontPaymentMethod
{
    public int StorefrontPaymentMethodId { get; set; }

    public int StorefrontId { get; set; }

    public int PaymentMethodId { get; set; }

    public  Storefront? Storefront { get; set; }

    public  PaymentMethod? PaymentMethod { get; set; }

    public bool IsActive { get; set; } = true;
    
    public void Validate()
    {
        if (Storefront != null &&
            Storefront.StorefrontId != StorefrontId)
        {
            throw new InvalidOperationException(
                "StorefrontId does not match the referenced storefront.");
        }

        if (PaymentMethod != null &&
            PaymentMethod.PaymentMethodId != PaymentMethodId)
        {
            throw new InvalidOperationException(
                "PaymentMethodId does not match the referenced payment method.");
        }
    }
}