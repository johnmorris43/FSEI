namespace FSEI.Domain.Evaluations;

public class EvaluationComponentAssessment
{
    public int EvaluationComponentAssessmentId { get; set; }
    
    public int? AvionicsEvaluationId { get; set; }

    public int EvaluationComponentId { get; set; }

    private int? _score;
    public int? Score
    {
        get => _score;
        private set => _score = value;
    }

    public string? Commentary { get; set; }
    
    public void SetScore(int? score)
    {
        if (score is null)
        {
            Score = null;
            return;
        }

        if (score < 0 || score > EvaluationComponent.MaximumScore)
        {
            throw new ArgumentOutOfRangeException(
                nameof(score),
                score,
                $"Score must be between 0 and {EvaluationComponent.MaximumScore} " +
                $"for {EvaluationComponent.Name}.");
        }

        Score = score;
    }

    public void ValidateMethodologyVersion(
        EvaluationMethodologyVersion evaluationMethodologyVersion)
    {
        if (EvaluationComponent.EvaluationMethodologyVersionId
            != evaluationMethodologyVersion.EvaluationMethodologyVersionId)
        {
            throw new InvalidOperationException(
                $"Component '{EvaluationComponent.Name}' does not belong to " +
                $"methodology version {evaluationMethodologyVersion.EvaluationMethodologyVersionId}.");
        }
    }
    
    public void ValidateEvaluationAssignment()
    {
        var hasSystemsDepthEvaluation = SystemsDepthEvaluationId.HasValue;
        var hasAvionicsEvaluation = AvionicsEvaluationId.HasValue;

        if (hasSystemsDepthEvaluation == hasAvionicsEvaluation)
        {
            throw new InvalidOperationException(
                "An evaluation component assessment must belong to exactly one evaluation.");
        }
    }
    
    public void Validate()
    {
        ValidateEvaluationAssignment();

        var methodologyVersion = SystemsDepthEvaluation is not null
            ? SystemsDepthEvaluation.EvaluationMethodologyVersion
            : AvionicsEvaluation!.EvaluationMethodologyVersion;

        ValidateMethodologyVersion(methodologyVersion);
    }
    public required EvaluationComponent EvaluationComponent { get; set; }
    
    public int? SystemsDepthEvaluationId { get; set; }
    
    public AvionicsEvaluation? AvionicsEvaluation { get; set; }

    public SystemsDepthEvaluation? SystemsDepthEvaluation { get; set; }
}