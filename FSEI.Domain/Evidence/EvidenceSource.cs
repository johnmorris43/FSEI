using FSEI.Domain.Evidence;

namespace FSEI.Domain.Evaluations;

public class EvaluationSource
{
    public int EvaluationSourceId { get; set; }

    public int SystemsDepthEvaluationId { get; set; }

    public int SourceId { get; set; }

    public int EvidenceRoleId { get; set; }

    public string? Notes { get; set; }

    public required SystemsDepthEvaluation SystemsDepthEvaluation { get; set; }

    public required Source Source { get; set; }

    public required EvidenceRole EvidenceRole { get; set; }
}