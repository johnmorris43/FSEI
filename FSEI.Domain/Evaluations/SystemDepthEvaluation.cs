using FSEI.Domain.Aircraft;
namespace FSEI.Domain.Evaluations;

public class SystemsDepthEvaluation
{
    private int? _electricalSystems;
    private int? _hydraulicPneumaticSystems;
    private int? _fmsNavigationDepth;
    private int? _failuresAbnormalProcedures;
    private int? _proceduralFidelity;
    public int SystemsDepthEvaluationId { get; set; }

    public int EvaluationScopeId { get; set; }
    
    public int? AircraftId { get; set; }

    public int? VariantId { get; set; }

    public int? AircraftConfigurationId { get; set; }

    public int? ElectricalSystems
    {
        get => _electricalSystems;
        set => _electricalSystems = ValidateScore(value, 20, nameof(ElectricalSystems));
    }

    public int? HydraulicPneumaticSystems
    {
        get => _hydraulicPneumaticSystems;
        set => _hydraulicPneumaticSystems =
            ValidateScore(value, 20, nameof(HydraulicPneumaticSystems));
    }

    public int? FmsNavigationDepth
    {
        get => _fmsNavigationDepth;
        set => _fmsNavigationDepth =
            ValidateScore(value, 20, nameof(FmsNavigationDepth));
    }

    public int? FailuresAbnormalProcedures
    {
        get => _failuresAbnormalProcedures;
        set => _failuresAbnormalProcedures =
            ValidateScore(value, 20, nameof(FailuresAbnormalProcedures));
    }

    public int? ProceduralFidelity
    {
        get => _proceduralFidelity;
        set => _proceduralFidelity =
            ValidateScore(value, 20, nameof(ProceduralFidelity));
    }
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
    public ICollection<SystemsDepthComponentAssessment> ComponentAssessments { get; set; } = [];
    
    private static int? ValidateScore(int? score, int maximum, string propertyName)
    {
        if (score is null)
        {
            return null;
        }

        if (score < 0 || score > maximum)
        {
            throw new ArgumentOutOfRangeException(
                propertyName,
                score,
                $"Score must be between 0 and {maximum}.");
        }

        return score;
    }
    
    public ICollection<EvaluationComponentAssessment> Assessments { get; set; } = [];
}