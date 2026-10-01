using FSEI.Domain.GeographicRegions;

namespace FSEI.Domain.Stores;

public class PriceObservation
{
    public int PriceObservationId { get; set; }

    public int CommercialOfferId { get; set; }

    public int CurrencyId { get; set; }
    
    public int? MarketId { get; set; }
    
    public int? OfferOptionId { get; set; }

    public OfferOption? OfferOption { get; set; }

    public Market? Market { get; set; }

    public decimal Amount { get; set; }

    public DateTime ObservedAt { get; set; }

    public required CommercialOffer CommercialOffer { get; set; }

    public required Currency Currency { get; set; }
    
    public void Validate()
    {
        if (CommercialOfferId != CommercialOffer.CommercialOfferId)
        {
            throw new InvalidOperationException(
                "A price observation's CommercialOfferId must match its CommercialOffer.");
        }

        if (CurrencyId != Currency.CurrencyId)
        {
            throw new InvalidOperationException(
                "A price observation's CurrencyId must match its Currency.");
        }
        
        if (Amount < 0)
        {
            throw new InvalidOperationException(
                "A price observation amount cannot be negative.");
        }
        
        if (ObservedAt == default)
        {
            throw new InvalidOperationException(
                "A price observation must have an observation date and time.");
        }
        
        if (MarketId.HasValue)
        {
            if (Market is null || MarketId.Value != Market.MarketId)
            {
                throw new InvalidOperationException(
                    "A price observation's MarketId must match its Market.");
            }
        }
        else if (Market is not null)
        {
            throw new InvalidOperationException(
                "A price observation cannot have a Market without a MarketId.");
        }
        
        if (OfferOptionId.HasValue)
        {
            if (OfferOption is null ||
                OfferOptionId.Value != OfferOption.OfferOptionId)
            {
                throw new InvalidOperationException(
                    "A price observation's OfferOptionId must match its OfferOption.");
            }

            if (OfferOption.CommercialOfferId != CommercialOfferId)
            {
                throw new InvalidOperationException(
                    "A price observation's OfferOption must belong to the same CommercialOffer.");
            }
        }
        else if (OfferOption is not null)
        {
            throw new InvalidOperationException(
                "A price observation cannot have an OfferOption without an OfferOptionId.");
        }
    }
}