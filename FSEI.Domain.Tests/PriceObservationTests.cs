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
}