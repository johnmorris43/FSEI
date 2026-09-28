using FSEI.Domain.Evaluations;

namespace FSEI.Domain.Evidence;

public class EvidenceRole
{
    public int EvidenceRoleId { get; set; }

    public required string Name { get; set; }

    public bool IsActive { get; set; } = true;
    
    public ICollection<EvaluationSource> SystemsDepthEvaluations { get; set; } = [];
    public ICollection<AvionicsEvaluationSource> AvionicsEvaluationSources { get; set; } = [];
}