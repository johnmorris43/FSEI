namespace FSEI.Domain.Evaluations;

public class SystemsDepthComponentAssessment
{
    public int SystemsDepthComponentAssessmentId { get; set; }

    public int SystemsDepthEvaluationId { get; set; }

    public required string ComponentName { get; set; }

    public int? Score { get; set; }

    public string? Commentary { get; set; }

    public required SystemsDepthEvaluation SystemsDepthEvaluation { get; set; }
}