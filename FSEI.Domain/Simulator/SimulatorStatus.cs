namespace FSEI.Domain.Simulator;

public class SimulatorStatus
{
    public int SimulatorStatusId { get; set; }
    public required string Code { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<Simulator> Simulators { get; set; } = [];
}