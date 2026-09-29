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

    public required EvaluationComponent EvaluationComponent { get; set; }
    
    public int? SystemsDepthEvaluationId { get; set; }
    
    public AvionicsEvaluation? AvionicsEvaluation { get; set; }

    public SystemsDepthEvaluation? SystemsDepthEvaluation { get; set; }
}