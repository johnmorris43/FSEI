using FSEI.Domain.Products;
using FSEI.Domain.Stores;

namespace FSEI.Domain.Tests;

public class OfferOptionTests
{
    [Test]
    public void Validate_WhenCommercialOfferMatches_DoesNotThrow()
    {
        var option = CreateOfferOption();

        Assert.DoesNotThrow(() => option.Validate());
    }

    [Test]
    public void Validate_WhenCommercialOfferDoesNotMatch_ThrowsInvalidOperationException()
    {
        var option = CreateOfferOption();

        option.CommercialOfferId = 2;

        Assert.Throws<InvalidOperationException>(() => option.Validate());
    }

    private static OfferOption CreateOfferOption()
    {
        var productType = new ProductType
        {
            ProductTypeId = 1,
            Code = "SERVICE",
            Name = "Service"
        };

        var product = new Product
        {
            ProductId = 1,
            ProductTypeId = 1,
            Name = "Test Navigation Product",
            ProductType = productType
        };

        var storefront = new Storefront
        {
            StorefrontId = 1,
            Name = "Test Store"
        };

        var commercialOffer = new CommercialOffer
        {
            CommercialOfferId = 1,
            ProductId = 1,
            StorefrontId = 1,
            Product = product,
            Storefront = storefront
        };

        return new OfferOption
        {
            OfferOptionId = 1,
            CommercialOfferId = 1,
            Name = "Monthly Subscription",
            CommercialOffer = commercialOffer
        };
    }
}