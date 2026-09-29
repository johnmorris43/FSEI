namespace FSEI.Domain.Evaluations;

public class EvaluationComponent
{
    public int EvaluationComponentId { get; set; }  
    
    public int EvaluationMethodologyVersionId { get; set; }

    public required string Name { get; set; }

    public int MaximumScore { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<EvaluationComponentAssessment> Assessments { get; set; } = [];
    public required EvaluationMethodologyVersion EvaluationMethodologyVersion { get; set; }
}