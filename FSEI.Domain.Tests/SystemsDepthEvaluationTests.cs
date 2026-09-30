using FSEI.Domain.Evaluations;

namespace FSEI.Domain.Tests;

public class SystemsDepthEvaluationTests
{
    [Test]
    public void ValidateEvaluationTarget_WhenNoTargetAssigned_ThrowsException()
    {
        var evaluation = CreateEvaluation(
            "VARIANT",
            "Variant");

        Assert.Throws<InvalidOperationException>(
            () => evaluation.ValidateEvaluationTarget());
    }
    
    [Test]
    public void ValidateEvaluationTarget_WhenMultipleTargetsAssigned_ThrowsException()
    {
        var evaluation = CreateEvaluation(
            "VARIANT",
            "Variant");

        evaluation.AircraftId = 1;
        evaluation.VariantId = 1;

        Assert.Throws<InvalidOperationException>(
            () => evaluation.ValidateEvaluationTarget());
    }
    
    [Test]
    public void ValidateEvaluationTarget_WhenVariantScopeMatchesVariantTarget_DoesNotThrow()
    {
        var evaluation = CreateEvaluation(
            "VARIANT",
            "Variant");

        evaluation.VariantId = 1;

        Assert.DoesNotThrow(
            () => evaluation.ValidateEvaluationTarget());
    }
    
    [Test]
    public void ValidateEvaluationTarget_WhenTargetDoesNotMatchScope_ThrowsException()
    {
        var evaluation = CreateEvaluation(
            "VARIANT",
            "Variant");

        evaluation.AircraftId = 1;
    }
    
    [Test]
    public void ValidateEvaluationTarget_WhenConfigurationScopeMatchesConfigurationTarget_DoesNotThrow()
    {
        var evaluation = CreateEvaluation(
            "CONFIGURATION",
            "Configuration");

        evaluation.AircraftConfigurationId = 1;
    }
    
    [Test]
    public void ValidateEvaluationTarget_WhenAircraftFamilyScopeMatchesAircraftTarget_DoesNotThrow()
    {
        var evaluation = CreateEvaluation(
            "AIRCRAFT_FAMILY",
            "Aircraft Family");

        evaluation.AircraftId = 1;
    }
    [Test]
    public void ValidateEvaluationTarget_WhenScopeCodeIsUnknown_ThrowsException()
    {
        var evaluation = CreateEvaluation(
            "UNKNOWN",
            "Unknown");

        evaluation.VariantId = 1;
    }
    
    [Test]
    public void SystemsDepthScore_WhenAllComponentsAreEvaluated_CalculatesNormalizedScore()
    {
        var evaluation = CreateEvaluation(
            "VARIANT",
            "Variant");

        evaluation.VariantId = 1;

        var scores = new[] { 19, 19, 20, 19, 20 };

        foreach (var score in scores)
        {
            evaluation.Assessments.Add(
                CreateAssessment(evaluation, 20, score));
        }

        Assert.That(
            evaluation.SystemsDepthScore,
            Is.EqualTo(97));
    }
    
    [Test]
    public void SystemsDepthScore_WhenComponentIsNotEvaluated_ExcludesItFromDenominator()
    {
        var evaluation = CreateEvaluation(
            "VARIANT",
            "Variant");

        evaluation.VariantId = 1;

        var scores = new int?[] { 20, 20, 20, 20, null };

        foreach (var score in scores)
        {
            evaluation.Assessments.Add(
                CreateAssessment(evaluation, 20, score));
        }

        Assert.That(
            evaluation.SystemsDepthScore,
            Is.EqualTo(100));
    }
    
    [Test]
    public void SystemsDepthScore_WhenComponentScoreIsZero_IncludesItInDenominator()
    {
        var evaluation = CreateEvaluation(
            "VARIANT",
            "Variant");

        evaluation.VariantId = 1;

        var scores = new int?[] { 20, 20, 20, 20, 0 };

        foreach (var score in scores)
        {
            var component = new EvaluationComponent
            {
                Name = "Test Component",
                MaximumScore = 20,
                EvaluationMethodologyVersion =
                    evaluation.EvaluationMethodologyVersion
            };

            var assessment = new EvaluationComponentAssessment
            {
                EvaluationComponent = component,
                SystemsDepthEvaluationId = 1
            };

            assessment.SetScore(score);

            evaluation.Assessments.Add(assessment);
        }

        Assert.That(
            evaluation.SystemsDepthScore,
            Is.EqualTo(80));
    }
    
    [Test]
    public void SystemsDepthScore_WhenNoComponentsAreEvaluated_ReturnsNull()
    {
        var evaluation = CreateEvaluation(
            "VARIANT",
            "Variant");

        evaluation.VariantId = 1;

        Assert.That(
            evaluation.SystemsDepthScore,
            Is.Null);
    }
    
    [Test]
    public void SystemsDepthScore_WhenResultIsMidpoint_RoundsAwayFromZero()
    {
        var evaluation = CreateEvaluation(
            "VARIANT",
            "Variant");

        evaluation.VariantId = 1;

        evaluation.Assessments.Add(
            CreateAssessment(evaluation, 8, 5));

        Assert.That(
            evaluation.SystemsDepthScore,
            Is.EqualTo(63));
    }
    
    
    
    private static EvaluationComponentAssessment CreateAssessment(
        SystemsDepthEvaluation evaluation,
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
            SystemsDepthEvaluationId = 1
        };

        assessment.SetScore(score);

        return assessment;
    }
    
    
    
    private static SystemsDepthEvaluation CreateEvaluation(
        string scopeCode,
        string scopeName)
    {
        return new SystemsDepthEvaluation
        {
            EvaluationScope = new EvaluationScope
            {
                Code = scopeCode,
                Name = scopeName
            },
            EvaluationMethodologyVersion = new EvaluationMethodologyVersion
            {
                EvaluationMethodology = new EvaluationMethodology
                {
                    Name = "Systems Depth"
                },
                VersionNumber = 1,
                EffectiveDate = DateTime.Today
            }
        };
    }
}