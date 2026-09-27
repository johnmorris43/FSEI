namespace FSEI.Domain.Aircraft;

public class Manufacturer
{
    public int ManufacturerId { get; set; }
    public required string Name { get; set; }
    public string? Notes { get; set; }
    public ICollection<Aircraft> Aircraft { get; set; } = [];
    public ICollection<Engine> Engines { get; set; } = [];
}