using FSEI.Domain.GeographicRegions;
using FSEI.Domain.Products;
using FSEI.Domain.Stores;

namespace FSEI.Domain.Tests;

public class PriceObservationTests
{
    [Test]
    public void Validate_WhenRelationshipsMatch_DoesNotThrow()
    {
        var observation = CreatePriceObservation();

        Assert.DoesNotThrow(() => observation.Validate());
    }

    [Test]
    public void Validate_WhenCommercialOfferDoesNotMatch_ThrowsInvalidOperationException()
    {
        var observation = CreatePriceObservation();

        observation.CommercialOfferId = 2;

        Assert.Throws<InvalidOperationException>(() => observation.Validate());
    }

    [Test]
    public void Validate_WhenCurrencyDoesNotMatch_ThrowsInvalidOperationException()
    {
        var observation = CreatePriceObservation();

        observation.CurrencyId = 2;

        Assert.Throws<InvalidOperationException>(() => observation.Validate());
    }

    private static PriceObservation CreatePriceObservation()
    {
        var productType = new ProductType
        {
            ProductTypeId = 1,
            Code = "BASE_PRODUCT",
            Name = "Base Product"
        };

        var product = new Product
        {
            ProductId = 1,
            ProductTypeId = 1,
            Name = "Test Product",
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

        var currency = new Currency
        {
            CurrencyId = 1,
            Code = "USD",
            Name = "US Dollar",
            Symbol = "$"
        };

        return new PriceObservation
        {
            PriceObservationId = 1,
            CommercialOfferId = 1,
            CurrencyId = 1,
            Amount = 69.99m,
            ObservedAt = DateTime.UtcNow,
            CommercialOffer = commercialOffer,
            Currency = currency
        };
    }
    
    [Test]
    public void Validate_WhenAmountIsZero_DoesNotThrow()
    {
        var observation = CreatePriceObservation();

        observation.Amount = 0m;

        Assert.DoesNotThrow(() => observation.Validate());
    }

    [Test]
    public void Validate_WhenAmountIsNegative_ThrowsInvalidOperationException()
    {
        var observation = CreatePriceObservation();

        observation.Amount = -1m;

        Assert.Throws<InvalidOperationException>(() => observation.Validate());
    }
    
    [Test]
    public void Validate_WhenObservedAtIsProvided_DoesNotThrow()
    {
        var observation = CreatePriceObservation();

        Assert.DoesNotThrow(() => observation.Validate());
    }

    [Test]
    public void Validate_WhenObservedAtIsNotProvided_ThrowsInvalidOperationException()
    {
        var observation = CreatePriceObservation();

        observation.ObservedAt = default;

        Assert.Throws<InvalidOperationException>(() => observation.Validate());
    }
    [Test]
    public void Validate_SameCommercialOfferCanHaveObservationsInDifferentCurrencies()
    {
        var euroObservation = CreatePriceObservation();
        euroObservation.CurrencyId = 2;
        euroObservation.Currency = new Currency
        {
            CurrencyId = 2,
            Code = "EUR",
            Name = "Euro",
            Symbol = "€"
        };
        euroObservation.Amount = 79.99m;

        var yenObservation = CreatePriceObservation();
        yenObservation.CurrencyId = 3;
        yenObservation.Currency = new Currency
        {
            CurrencyId = 3,
            Code = "JPY",
            Name = "Japanese Yen",
            Symbol = "¥"
        };
        yenObservation.Amount = 9700m;

        Assert.Multiple(() =>
        {
            Assert.DoesNotThrow(() => euroObservation.Validate());
            Assert.DoesNotThrow(() => yenObservation.Validate());

            Assert.That(
                euroObservation.CommercialOfferId,
                Is.EqualTo(yenObservation.CommercialOfferId));

            Assert.That(
                euroObservation.CurrencyId,
                Is.Not.EqualTo(yenObservation.CurrencyId));
        });
    }
    
    [Test]
    public void Validate_WhenMarketIsNotProvided_DoesNotThrow()
    {
        var observation = CreatePriceObservation();

        Assert.DoesNotThrow(() => observation.Validate());
    }

    [Test]
    public void Validate_WhenMarketMatches_DoesNotThrow()
    {
        var observation = CreatePriceObservation();

        observation.MarketId = 1;
        observation.Market = CreateMarket(1);

        Assert.DoesNotThrow(() => observation.Validate());
    }

    [Test]
    public void Validate_WhenMarketIdDoesNotMatchMarket_ThrowsInvalidOperationException()
    {
        var observation = CreatePriceObservation();

        observation.MarketId = 2;
        observation.Market = CreateMarket(1);

        Assert.Throws<InvalidOperationException>(() => observation.Validate());
    }

    [Test]
    public void Validate_WhenMarketExistsWithoutMarketId_ThrowsInvalidOperationException()
    {
        var observation = CreatePriceObservation();

        observation.Market = CreateMarket(1);

        Assert.Throws<InvalidOperationException>(() => observation.Validate());
    }
    
    [Test]
    public void Validate_WhenMarketIdExistsWithoutMarket_ThrowsInvalidOperationException()
    {
        var observation = CreatePriceObservation();

        observation.MarketId = 1;
        observation.Market = null;

        Assert.Throws<InvalidOperationException>(() => observation.Validate());
    }
    
    [Test]
public void Validate_WhenOfferOptionIsNotProvided_DoesNotThrow()
{
    var observation = CreatePriceObservation();

    Assert.DoesNotThrow(() => observation.Validate());
}

[Test]
public void Validate_WhenOfferOptionMatches_DoesNotThrow()
{
    var observation = CreatePriceObservation();
    var acquisitionType = CreateAcquisitionType();
    var option = new OfferOption
    {
        AcquisitionTypeId = acquisitionType.AcquisitionTypeId,
        AcquisitionType = acquisitionType,
        OfferOptionId = 1,
        CommercialOfferId = observation.CommercialOfferId,
        Name = "Monthly Subscription",
        CommercialOffer = observation.CommercialOffer
    };

    observation.OfferOptionId = 1;
    observation.OfferOption = option;

    Assert.DoesNotThrow(() => observation.Validate());
}

[Test]
public void Validate_WhenOfferOptionIdDoesNotMatchOfferOption_ThrowsInvalidOperationException()
{
    var observation = CreatePriceObservation();
    var acquisitionType = CreateAcquisitionType();
    var option = new OfferOption
    {
        AcquisitionTypeId = acquisitionType.AcquisitionTypeId,
        AcquisitionType = acquisitionType,
        OfferOptionId = 1,
        CommercialOfferId = observation.CommercialOfferId,
        Name = "Monthly Subscription",
        CommercialOffer = observation.CommercialOffer
    };

    observation.OfferOptionId = 2;
    observation.OfferOption = option;

    Assert.Throws<InvalidOperationException>(() => observation.Validate());
}

[Test]
public void Validate_WhenOfferOptionBelongsToDifferentCommercialOffer_ThrowsInvalidOperationException()
{
    var observation = CreatePriceObservation();

    var otherOffer = new CommercialOffer
    {
        CommercialOfferId = 2,
        ProductId = observation.CommercialOffer.ProductId,
        StorefrontId = observation.CommercialOffer.StorefrontId,
        Product = observation.CommercialOffer.Product,
        Storefront = observation.CommercialOffer.Storefront
    };
    var acquisitionType = CreateAcquisitionType();
    var option = new OfferOption
    {
        AcquisitionTypeId = acquisitionType.AcquisitionTypeId,
        AcquisitionType = acquisitionType,
        OfferOptionId = 1,
        CommercialOfferId = 2,
        Name = "Monthly Subscription",
        CommercialOffer = otherOffer
    };

    observation.OfferOptionId = 1;
    observation.OfferOption = option;

    Assert.Throws<InvalidOperationException>(() => observation.Validate());
}

[Test]
public void Validate_WhenOfferOptionExistsWithoutOfferOptionId_ThrowsInvalidOperationException()
{
    var observation = CreatePriceObservation();
    var acquisitionType = CreateAcquisitionType();
    observation.OfferOption = new OfferOption
    {
        AcquisitionTypeId = acquisitionType.AcquisitionTypeId,
        AcquisitionType = acquisitionType,
        OfferOptionId = 1,
        CommercialOfferId = observation.CommercialOfferId,
        Name = "Monthly Subscription",
        CommercialOffer = observation.CommercialOffer
    };

    observation.OfferOptionId = null;

    Assert.Throws<InvalidOperationException>(() => observation.Validate());
}

[Test]
public void Validate_SameCommercialOfferCanHaveMonthlyAndAnnualPricedOptions()
{
    var monthlyObservation = CreatePriceObservation();
    var annualObservation = CreatePriceObservation();

    var commercialOffer = monthlyObservation.CommercialOffer;

    annualObservation.CommercialOfferId = commercialOffer.CommercialOfferId;
    annualObservation.CommercialOffer = commercialOffer;
    var acquisitionType = CreateAcquisitionType();
    var monthlyOption = new OfferOption
    {
        AcquisitionTypeId = acquisitionType.AcquisitionTypeId,
        AcquisitionType = acquisitionType,
        OfferOptionId = 1,
        CommercialOfferId = commercialOffer.CommercialOfferId,
        Name = "Monthly Subscription",
        CommercialOffer = commercialOffer
    };
    
    var annualOption = new OfferOption
    {
        AcquisitionTypeId = acquisitionType.AcquisitionTypeId,
        AcquisitionType = acquisitionType,
        OfferOptionId = 2,
        CommercialOfferId = commercialOffer.CommercialOfferId,
        Name = "Annual Subscription",
        CommercialOffer = commercialOffer
    };

    monthlyObservation.OfferOptionId = monthlyOption.OfferOptionId;
    monthlyObservation.OfferOption = monthlyOption;
    monthlyObservation.Amount = 9.05m;

    annualObservation.OfferOptionId = annualOption.OfferOptionId;
    annualObservation.OfferOption = annualOption;
    annualObservation.Amount = 81.64m;

    Assert.Multiple(() =>
    {
        Assert.DoesNotThrow(() => monthlyObservation.Validate());
        Assert.DoesNotThrow(() => annualObservation.Validate());

        Assert.That(
            monthlyObservation.CommercialOfferId,
            Is.EqualTo(annualObservation.CommercialOfferId));

        Assert.That(
            monthlyObservation.OfferOptionId,
            Is.Not.EqualTo(annualObservation.OfferOptionId));

        Assert.That(
            monthlyObservation.Amount,
            Is.Not.EqualTo(annualObservation.Amount));
    });
}
    
    private static Market CreateMarket(int marketId)
    {
        var region = new Region
        {
            RegionId = 1,
            Code = "DE",
            Name = "Germany"
        };

        return new Market
        {
            MarketId = marketId,
            RegionId = 1,
            Code = "DE",
            Name = "German Market",
            Region = region
        };
    }
    
    private static AcquisitionType CreateAcquisitionType()
    {
        return new AcquisitionType
        {
            AcquisitionTypeId = 1,
            Code = "PURCHASE",
            Name = "Purchase"
        };
    }
}