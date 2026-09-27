namespace FSEI.Domain.Avionics;

public class Avionics
{
    public int AvionicsId { get; set; }

    public int AvionicsTypeId { get; set; }

    public required string DeveloperManufacturer { get; set; }

    public required string SystemModel { get; set; }

    public string? VersionGeneration { get; set; }

    public string? Notes { get; set; }

    public required AvionicsType AvionicsType { get; set; }
}