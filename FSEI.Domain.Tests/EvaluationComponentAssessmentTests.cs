using FSEI.Domain.Evaluations;


namespace FSEI.Domain.Tests;

public class EvaluationComponentAssessmentTests
{
    [Test]
    public void SetScore_WhenScoreIsWithinMaximum_SetsScore()
    {
        var component = CreateComponent();
        var assessment = new EvaluationComponentAssessment
        {
            EvaluationComponent = component
        };

        assessment.SetScore(19);

        Assert.That(assessment.Score, Is.EqualTo(19));
    }
    [Test]
    public void SetScore_WhenScoreExceedsMaximum_ThrowsException()
    {
        var component = CreateComponent();
        var assessment = new EvaluationComponentAssessment
        {
            EvaluationComponent = component
        };

        Assert.Throws<ArgumentOutOfRangeException>(
            () => assessment.SetScore(21));
    }
    
    [Test]
    public void SetScore_WhenScoreIsNull_SetsScoreToNull()
    {
        var component = CreateComponent();
        var assessment = new EvaluationComponentAssessment
        {
            EvaluationComponent = component
        };

        assessment.SetScore(19);
        assessment.SetScore(null);

        Assert.That(assessment.Score, Is.Null);
    }
    
    [Test]
    public void ValidateEvaluationAssignment_WhenNoEvaluationAssigned_ThrowsException()
    {
        var component = CreateComponent();

        var assessment = new EvaluationComponentAssessment
        {
            EvaluationComponent = component
        };

        Assert.Throws<InvalidOperationException>(
            () => assessment.ValidateEvaluationAssignment());
    }
    
    [Test]
    public void ValidateEvaluationAssignment_WhenBothEvaluationsAssigned_ThrowsException()
    {
        var component = CreateComponent();

        var assessment = new EvaluationComponentAssessment
        {
            EvaluationComponent = component,
            SystemsDepthEvaluationId = 1,
            AvionicsEvaluationId = 1
        };

        Assert.Throws<InvalidOperationException>(
            () => assessment.ValidateEvaluationAssignment());
    }
    [Test]
    public void ValidateEvaluationAssignment_WhenOneEvaluationAssigned_DoesNotThrow()
    {
        var component = CreateComponent();

        var assessment = new EvaluationComponentAssessment
        {
            EvaluationComponent = component,
            SystemsDepthEvaluationId = 1
        };

        Assert.DoesNotThrow(
            () => assessment.ValidateEvaluationAssignment());
    }
    [Test]
    public void ValidateMethodologyVersion_WhenComponentBelongsToDifferentVersion_ThrowsException()
    {
        var component = CreateComponent();

        var differentMethodologyVersion = new EvaluationMethodologyVersion
        {
            EvaluationMethodologyId = 1,
            VersionNumber = 2,
            EffectiveDate = DateTime.Today,
            EvaluationMethodology = new EvaluationMethodology
            {
                Name = "Systems Depth"
            }
        };

        component.EvaluationMethodologyVersionId = 1;
        differentMethodologyVersion.EvaluationMethodologyVersionId = 2;

        var assessment = new EvaluationComponentAssessment
        {
            EvaluationComponent = component
        };

        Assert.Throws<InvalidOperationException>(
            () => assessment.ValidateMethodologyVersion(
                differentMethodologyVersion));
    }
    [Test]
    public void ValidateMethodologyVersion_WhenComponentBelongsToSameVersion_DoesNotThrow()
    {
        var component = CreateComponent();

        component.EvaluationMethodologyVersionId = 1;
        component.EvaluationMethodologyVersion.EvaluationMethodologyVersionId = 1;

        var assessment = new EvaluationComponentAssessment
        {
            EvaluationComponent = component
        };

        Assert.DoesNotThrow(
            () => assessment.ValidateMethodologyVersion(
                component.EvaluationMethodologyVersion));
    }
   
    [Test]
    public void SetScore_WhenScoreIsBelowZero_ThrowsException()
    {
        var component = CreateComponent();

        var assessment = new EvaluationComponentAssessment
        {
            EvaluationComponent = component
        };

        Assert.Throws<ArgumentOutOfRangeException>(
            () => assessment.SetScore(-1));
    }
    
    private static EvaluationComponent CreateComponent(
        int maximumScore = 20)
    {
        var methodology = new EvaluationMethodology
        {
            Name = "Systems Depth"
        };

        var methodologyVersion = new EvaluationMethodologyVersion
        {
            EvaluationMethodologyId = 1,
            VersionNumber = 1,
            EffectiveDate = DateTime.Today,
            EvaluationMethodology = methodology
        };

        return new EvaluationComponent
        {
            Name = "Electrical Systems",
            MaximumScore = maximumScore,
            EvaluationMethodologyVersion = methodologyVersion
        };
    }
}