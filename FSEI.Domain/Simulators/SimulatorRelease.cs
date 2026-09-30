namespace FSEI.Domain.Simulators;

public class SimulatorRelease
{
    public int SimulatorReleaseId { get; set; }
    public int SimulatorId { get; set; }

    public required string Version { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public string? Notes { get; set; }

    public required Simulator Simulator { get; set; }
}