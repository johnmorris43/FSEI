namespace FSEI.Domain.Tests;
using FSEI.Domain.Evaluations;

public class AvionicsEvaluationTests
{

    [Test]
    public void FidelityScore_WhenAllComponentsAreEvaluated_CalculatesNormalizedScore()
    {
        var evaluation = CreateEvaluation();

        evaluation.Assessments.Add(
            CreateAssessment(evaluation, 25, 25));

        evaluation.Assessments.Add(
            CreateAssessment(evaluation, 20, 19));

        evaluation.Assessments.Add(
            CreateAssessment(evaluation, 15, 14));

        evaluation.Assessments.Add(
            CreateAssessment(evaluation, 20, 19));

        evaluation.Assessments.Add(
            CreateAssessment(evaluation, 8, 8));

        evaluation.Assessments.Add(
            CreateAssessment(evaluation, 7, 6));

        evaluation.Assessments.Add(
            CreateAssessment(evaluation, 5, 5));

        Assert.That(
            evaluation.FidelityScore,
            Is.EqualTo(96));
    }

    [Test]
    public void FidelityScore_WhenComponentIsNotEvaluated_ExcludesItFromDenominator()
    {
        var evaluation = CreateEvaluation();

        evaluation.Assessments.Add(
            CreateAssessment(evaluation, 25, 25));

        evaluation.Assessments.Add(
            CreateAssessment(evaluation, 20, 20));

        evaluation.Assessments.Add(
            CreateAssessment(evaluation, 15, null));

        Assert.That(
            evaluation.FidelityScore,
            Is.EqualTo(100));
    }
    
    [Test]
    public void FidelityScore_WhenComponentScoreIsZero_IncludesItInDenominator()
    {
        var evaluation = CreateEvaluation();

        evaluation.Assessments.Add(
            CreateAssessment(evaluation, 25, 25));

        evaluation.Assessments.Add(
            CreateAssessment(evaluation, 20, 20));

        evaluation.Assessments.Add(
            CreateAssessment(evaluation, 15, 0));

        Assert.That(
            evaluation.FidelityScore,
            Is.EqualTo(75));
    }
    
    [Test]
    public void FidelityScore_WhenNoComponentsAreEvaluated_ReturnsNull()
    {
        var evaluation = CreateEvaluation();

        Assert.That(
            evaluation.FidelityScore,
            Is.Null);
    }

    private static AvionicsEvaluation CreateEvaluation()
    {
        var methodology = new EvaluationMethodology
        {
            Name = "Avionics Fidelity"
        };

        var methodologyVersion = new EvaluationMethodologyVersion
        {
            EvaluationMethodologyId = 1,
            VersionNumber = 1,
            EffectiveDate = DateTime.Today,
            EvaluationMethodology = methodology
        };

        return new AvionicsEvaluation
        {
            EvaluationMethodologyVersion = methodologyVersion,
            ConfigurationAvionics = null!
        };
    }

    private static EvaluationComponentAssessment CreateAssessment(
        AvionicsEvaluation evaluation,
        int maximumScore,
        int? score)
    {
        var component = new EvaluationComponent
        {
            Name = "Test Component",
            MaximumScore = maximumScore,
            EvaluationMethodologyVersion =
                evaluation.EvaluationMethodologyVersion
        };

        var assessment = new EvaluationComponentAssessment
        {
            EvaluationComponent = component,
            AvionicsEvaluationId = 1
        };

        assessment.SetScore(score);

        return assessment;
    }


    
}