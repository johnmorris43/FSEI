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