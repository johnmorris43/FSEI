using FSEI.Domain.Avionics;

namespace FSEI.Domain.Evaluations;

public class AvionicsEvaluation
{
    
    
    public int AvionicsEvaluationId { get; set; }

    public int ConfigurationAvionicsId { get; set; }
    
    public int EvaluationMethodologyVersionId { get; set; }

    public DateTime? EvaluationDate { get; set; }
    
    public int? FidelityScore
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
    public string? Notes { get; set; }
    
    public ICollection<AvionicsEvaluationSource> Sources { get; set; } = [];
    public ICollection<EvaluationComponentAssessment> Assessments { get; set; } = [];
    
    public required EvaluationMethodologyVersion EvaluationMethodologyVersion { get; set; }

    public required ConfigurationAvionics ConfigurationAvionics { get; set; }
}