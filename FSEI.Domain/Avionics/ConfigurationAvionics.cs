namespace FSEI.Domain.Avionics;
using FSEI.Domain.Aircraft;
using FSEI.Domain.Evaluations;
public class ConfigurationAvionics
{
    public int ConfigurationAvionicsId { get; set; }

    public int AircraftConfigurationId { get; set; }

    public int AvionicsSystemId { get; set; }

    public string? Role { get; set; }

    public string? Notes { get; set; }

    public required AircraftConfiguration AircraftConfiguration { get; set; }

    public required AvionicsSystem AvionicsSystem { get; set; }

    public ICollection<AvionicsEvaluation>? AvionicsEvaluations { get; set; } = []; 
}