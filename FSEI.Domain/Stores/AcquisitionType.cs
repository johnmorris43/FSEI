namespace FSEI.Domain.Stores;

public class AcquisitionType
{
    public int AcquisitionTypeId { get; set; }

    public required string Code { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;
}