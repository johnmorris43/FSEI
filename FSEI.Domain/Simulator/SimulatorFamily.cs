namespace FSEI.Domain.Simulator;

public class SimulatorFamily
{
    public int SimulatorFamilyId { get; set; }
    public required string Name { get; set; }
    public string? Notes { get; set; }

    public ICollection<Simulator> Simulators { get; set; } = [];
}