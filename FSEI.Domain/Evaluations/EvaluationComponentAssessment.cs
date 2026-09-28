namespace FSEI.Domain.Evaluations;

public class EvaluationComponentAssessment
{
    public int EvaluationComponentAssessmentId { get; set; }

    public int EvaluationComponentId { get; set; }

    public int? Score { get; set; }

    public string? Commentary { get; set; }

    public required EvaluationComponent EvaluationComponent { get; set; }
    
    public int? SystemsDepthEvaluationId { get; set; }

    public SystemsDepthEvaluation? SystemsDepthEvaluation { get; set; }
}