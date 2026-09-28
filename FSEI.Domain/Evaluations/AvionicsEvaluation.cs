using FSEI.Domain.Avionics;

namespace FSEI.Domain.Evaluations;

public class AvionicsEvaluation
{
    private int? _functionalFidelity;
    private int? _operationalProceduralFidelity;
    private int? _displayInterfaceFidelity;
    private int? _systemLogicIntegration;
    private int? _navigationDataHandling;
    private int? _failuresAbnormalBehavior;
    private int? _depthEdgeCaseFidelity;
    public int AvionicsEvaluationId { get; set; }

    public int ConfigurationAvionicsId { get; set; }

    public DateTime? EvaluationDate { get; set; }
    public int? FunctionalFidelity
    {
        get => _functionalFidelity;
        set => _functionalFidelity =
            ValidateScore(value, 25, nameof(FunctionalFidelity));
    }

    public int? OperationalProceduralFidelity
    {
        get => _operationalProceduralFidelity;
        set => _operationalProceduralFidelity =
            ValidateScore(value, 20, nameof(OperationalProceduralFidelity));
    }

    public int? DisplayInterfaceFidelity
    {
        get => _displayInterfaceFidelity;
        set => _displayInterfaceFidelity =
            ValidateScore(value, 15, nameof(DisplayInterfaceFidelity));
    }

    public int? SystemLogicIntegration
    {
        get => _systemLogicIntegration;
        set => _systemLogicIntegration =
            ValidateScore(value, 20, nameof(SystemLogicIntegration));
    }

    public int? NavigationDataHandling
    {
        get => _navigationDataHandling;
        set => _navigationDataHandling =
            ValidateScore(value, 8, nameof(NavigationDataHandling));
    }

    public int? FailuresAbnormalBehavior
    {
        get => _failuresAbnormalBehavior;
        set => _failuresAbnormalBehavior =
            ValidateScore(value, 7, nameof(FailuresAbnormalBehavior));
    }

    public int? DepthEdgeCaseFidelity
    {
        get => _depthEdgeCaseFidelity;
        set => _depthEdgeCaseFidelity =
            ValidateScore(value, 5, nameof(DepthEdgeCaseFidelity));
    }
    public int? FidelityScore
    {
        get
        {
            (int? Score, int Maximum)[] components =
            [
                (FunctionalFidelity, 25),
                (OperationalProceduralFidelity, 20),
                (DisplayInterfaceFidelity, 15),
                (SystemLogicIntegration, 20),
                (NavigationDataHandling, 8),
                (FailuresAbnormalBehavior, 7),
                (DepthEdgeCaseFidelity, 5)
            ];

            var evaluatedComponents = components
                .Where(component => component.Score.HasValue)
                .ToList();

            if (evaluatedComponents.Count == 0)
            {
                return null;
            }

            var totalScore = evaluatedComponents.Sum(
                component => component.Score!.Value);

            var maximumPossibleScore = evaluatedComponents.Sum(
                component => component.Maximum);

            return (int)Math.Round(
                (double)totalScore / maximumPossibleScore * 100,
                MidpointRounding.AwayFromZero);
        }
    }

    public string? Notes { get; set; }
    
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
    public ICollection<AvionicsEvaluationSource> Sources { get; set; } = [];

    public required ConfigurationAvionics ConfigurationAvionics { get; set; }
}