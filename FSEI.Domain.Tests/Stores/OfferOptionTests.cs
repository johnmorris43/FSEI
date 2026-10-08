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
    
    [Test]
    public void Validate_WhenAcquisitionTypeMatches_DoesNotThrow()
    {
        var option = CreateOfferOption();

        Assert.DoesNotThrow(() => option.Validate());
    }

    [Test]
    public void Validate_WhenAcquisitionTypeDoesNotMatch_ThrowsInvalidOperationException()
    {
        var option = CreateOfferOption();

        option.AcquisitionTypeId = 2;

        Assert.Throws<InvalidOperationException>(() => option.Validate());
    }
    
    [Test]
    public void Validate_WhenBillingIntervalMatches_DoesNotThrow()
    {
        var offerOption = CreateOfferOption();

        offerOption.BillingIntervalId = 1;
        offerOption.BillingInterval = new BillingInterval
        {
            BillingIntervalId = 1,
            Code = "MONTHLY",
            Name = "Monthly"
        };

        Assert.DoesNotThrow(() => offerOption.Validate());
    }
    
    [Test]
    public void Validate_WhenBillingIntervalDoesNotMatch_ThrowsInvalidOperationException()
    {
        var offerOption = CreateOfferOption();

        offerOption.BillingIntervalId = 1;
        offerOption.BillingInterval = new BillingInterval
        {
            BillingIntervalId = 2,
            Code = "MONTHLY",
            Name = "Monthly"
        };

        Assert.Throws<InvalidOperationException>(() => offerOption.Validate());
    }
    
    [Test]
    public void Validate_WhenBillingIntervalIdExistsWithoutBillingInterval_ThrowsInvalidOperationException()
    {
        var offerOption = CreateOfferOption();

        offerOption.BillingIntervalId = 1;
        offerOption.BillingInterval = null;

        Assert.Throws<InvalidOperationException>(() => offerOption.Validate());
    }
    
    [Test]
    public void Validate_WhenBillingIntervalExistsWithoutBillingIntervalId_ThrowsInvalidOperationException()
    {
        var offerOption = CreateOfferOption();

        offerOption.BillingIntervalId = null;
        offerOption.BillingInterval = new BillingInterval
        {
            BillingIntervalId = 1,
            Code = "MONTHLY",
            Name = "Monthly"
        };

        Assert.Throws<InvalidOperationException>(() => offerOption.Validate());
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

        var acquisitionType = new AcquisitionType
        {
            AcquisitionTypeId = 1,
            Code = "SUBSCRIPTION",
            Name = "Subscription"
        };
        return new OfferOption
        {
            AcquisitionTypeId = acquisitionType.AcquisitionTypeId,
            AcquisitionType = acquisitionType,
            OfferOptionId = 1,
            CommercialOfferId = 1,
            Name = "Monthly Subscription",
            CommercialOffer = commercialOffer
        };
    }
}