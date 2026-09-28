using FSEI.Domain.Evidence;

namespace FSEI.Domain.Evaluations;

public class AvionicsEvaluationSource
{
    public int AvionicsEvaluationSourceId { get; set; }

    public int AvionicsEvaluationId { get; set; }

    public int SourceId { get; set; }

    public int EvidenceRoleId { get; set; }

    public string? Notes { get; set; }

    public required AvionicsEvaluation AvionicsEvaluation { get; set; }

    public required Source Source { get; set; }

    public required EvidenceRole EvidenceRole { get; set; }
}