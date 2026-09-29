using FSEI.Domain.Aircraft;
namespace FSEI.Domain.Evaluations;

public class SystemsDepthEvaluation
{
    
    public int SystemsDepthEvaluationId { get; set; }
    
    public int EvaluationMethodologyVersionId { get; set; }

    public int EvaluationScopeId { get; set; }
    
    public int? AircraftId { get; set; }

    public int? VariantId { get; set; }

    public int? AircraftConfigurationId { get; set; }

    
    public int? SystemsDepthScore
    {
        get
        {
            var evaluatedAssessments = Assessments
                .Where(assessment => assessment.Score.HasValue)
                .ToList();

            if (evaluatedAssessments.Count == 0)
            {
                return null;
            }

            var totalScore = evaluatedAssessments.Sum(
                assessment => assessment.Score!.Value);

            var maximumPossibleScore = evaluatedAssessments.Sum(
                assessment => assessment.EvaluationComponent.MaximumScore);

            if (maximumPossibleScore == 0)
            {
                return null;
            }

            return (int)Math.Round(
                (double)totalScore / maximumPossibleScore * 100,
                MidpointRounding.AwayFromZero);
        }
    }

    public DateTime? DateVerified { get; set; }

    public string? Notes { get; set; }

    public Aircraft.Aircraft? Aircraft { get; set; }

    public Variant? Variant { get; set; }

    public AircraftConfiguration? AircraftConfiguration { get; set; }
    public required EvaluationScope EvaluationScope { get; set; }
    public ICollection<EvaluationSource> Sources { get; set; } = [];
    
    public void ValidateEvaluationTarget()
    {
        var targetCount = 0;

        if (AircraftId.HasValue)
            targetCount++;

        if (VariantId.HasValue)
            targetCount++;

        if (AircraftConfigurationId.HasValue)
            targetCount++;

        if (targetCount != 1)
        {
            throw new InvalidOperationException(
                "A systems depth evaluation must target exactly one aircraft, variant, or aircraft configuration.");
        }

        var targetMatchesScope = EvaluationScope.Code switch
        {
            "AIRCRAFT_FAMILY" => AircraftId.HasValue,
            "VARIANT" => VariantId.HasValue,
            "CONFIGURATION" => AircraftConfigurationId.HasValue,
            _ => throw new InvalidOperationException(
                $"Unknown evaluation scope code '{EvaluationScope.Code}'.")
        };

        if (!targetMatchesScope)
        {
            throw new InvalidOperationException(
                $"The selected evaluation target does not match evaluation scope '{EvaluationScope.Name}'.");
        }
    }
    
    public ICollection<EvaluationComponentAssessment> Assessments { get; set; } = [];
    
    public required EvaluationMethodologyVersion EvaluationMethodologyVersion { get; set; }
}