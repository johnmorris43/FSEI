namespace FSEI.Domain.Stores;

public class OfferOption
{
    public int OfferOptionId { get; set; }

    public int CommercialOfferId { get; set; }
    
    public int AcquisitionTypeId { get; set; }

    public required AcquisitionType AcquisitionType { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public required CommercialOffer CommercialOffer { get; set; }
    
    public void Validate()
    {
        if (CommercialOfferId != CommercialOffer.CommercialOfferId)
        {
            throw new InvalidOperationException(
                "An offer option's CommercialOfferId must match its CommercialOffer.");
        }
        
        if (AcquisitionTypeId != AcquisitionType.AcquisitionTypeId)
        {
            throw new InvalidOperationException(
                "An offer option's AcquisitionTypeId must match its AcquisitionType.");
        }
    }
}