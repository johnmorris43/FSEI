using FSEI.Domain.Payment;
using FSEI.Domain.Stores;
using NUnit.Framework;

namespace FSEI.Domain.Tests.Stores;

[TestFixture]
public class StorefrontPaymentMethodTests
{
    [Test]
    public void StorefrontPaymentMethod_WithValidReferences_IsCreated()
    {
        var storefront = new Storefront
        {
            StorefrontId = 1,
            Name = "X-Plane.org Store"
        };

        var paymentMethod = new PaymentMethod
        {
            PaymentMethodId = 1,
            Code = "PAYPAL",
            Name = "PayPal"
        };

        var storefrontPaymentMethod = new StorefrontPaymentMethod
        {
            StorefrontPaymentMethodId = 1,
            StorefrontId = storefront.StorefrontId,
            PaymentMethodId = paymentMethod.PaymentMethodId,
            Storefront = storefront,
            PaymentMethod = paymentMethod
        };

        Assert.Multiple(() =>
        {
            Assert.That(
                storefrontPaymentMethod.StorefrontId,
                Is.EqualTo(storefront.StorefrontId));

            Assert.That(
                storefrontPaymentMethod.PaymentMethodId,
                Is.EqualTo(paymentMethod.PaymentMethodId));

            Assert.That(
                storefrontPaymentMethod.IsActive,
                Is.True);
        });
    }
    
    [Test]
    public void Validate_WhenStorefrontIdDoesNotMatch_ThrowsInvalidOperationException()
    {
        var storefront = new Storefront
        {
            StorefrontId = 10,
            Name = "X-Plane.org Store"
        };

        var paymentMethod = new PaymentMethod
        {
            PaymentMethodId = 1,
            Code = "PAYPAL",
            Name = "PayPal"
        };

        var relationship = new StorefrontPaymentMethod
        {
            StorefrontPaymentMethodId = 1,
            StorefrontId = 99,
            PaymentMethodId = 1,
            Storefront = storefront,
            PaymentMethod = paymentMethod
        };

        Assert.Throws<InvalidOperationException>(
            () => relationship.Validate());
    }
    [Test]
    public void Validate_WhenPaymentMethodIdDoesNotMatch_ThrowsInvalidOperationException()
    {
        var storefront = new Storefront
        {
            StorefrontId = 10,
            Name = "X-Plane.org Store"
        };

        var paymentMethod = new PaymentMethod
        {
            PaymentMethodId = 1,
            Code = "PAYPAL",
            Name = "PayPal"
        };

        var relationship = new StorefrontPaymentMethod
        {
            StorefrontPaymentMethodId = 1,
            StorefrontId = storefront.StorefrontId,
            PaymentMethodId = 99,
            Storefront = storefront,
            PaymentMethod = paymentMethod
        };

        Assert.Throws<InvalidOperationException>(
            () => relationship.Validate());
    }
    [Test]
    public void Validate_WhenReferencesMatch_DoesNotThrow()
    {
        var storefront = new Storefront
        {
            StorefrontId = 10,
            Name = "X-Plane.org Store"
        };

        var paymentMethod = new PaymentMethod
        {
            PaymentMethodId = 1,
            Code = "PAYPAL",
            Name = "PayPal"
        };

        var relationship = new StorefrontPaymentMethod
        {
            StorefrontPaymentMethodId = 1,
            StorefrontId = storefront.StorefrontId,
            PaymentMethodId = paymentMethod.PaymentMethodId,
            Storefront = storefront,
            PaymentMethod = paymentMethod
        };

        Assert.DoesNotThrow(() => relationship.Validate());
    }
    
    [Test]
    public void Validate_WhenNavigationPropertiesAreNull_DoesNotThrow()
    {
        var relationship = new StorefrontPaymentMethod
        {
            StorefrontPaymentMethodId = 1,
            StorefrontId = 10,
            PaymentMethodId = 1
        };

        Assert.Multiple(() =>
        {
            Assert.That(relationship.Storefront, Is.Null);
            Assert.That(relationship.PaymentMethod, Is.Null);
            Assert.DoesNotThrow(() => relationship.Validate());
        });
    }
}