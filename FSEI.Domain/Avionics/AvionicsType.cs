namespace FSEI.Domain.Avionics;

public class AvionicsType
{
    public int AvionicsTypeId { get; set; }

    public required string Name { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<Avionics> AvionicsSystems { get; set; } = [];

}