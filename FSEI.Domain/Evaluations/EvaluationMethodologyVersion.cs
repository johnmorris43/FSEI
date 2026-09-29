namespace FSEI.Domain.Evaluations;

public class EvaluationMethodologyVersion
{
    public int EvaluationMethodologyVersionId { get; set; }

    public int EvaluationMethodologyId { get; set; }

    public int VersionNumber { get; set; }

    public DateTime EffectiveDate { get; set; }

    public string? Notes { get; set; }

    public bool IsActive { get; set; } = true;

    public required EvaluationMethodology EvaluationMethodology { get; set; }

    public ICollection<EvaluationComponent> Components { get; set; } = [];
}