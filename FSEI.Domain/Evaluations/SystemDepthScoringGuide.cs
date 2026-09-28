namespace FSEI.Domain.Evaluations;

public class SystemsDepthScoreGuidance
{
    public int SystemsDepthScoreGuidanceId { get; set; }

    public int MinimumScore { get; set; }

    public int MaximumScore { get; set; }

    public required string Label { get; set; }

    public required string Description { get; set; }

    public bool IsActive { get; set; } = true;
}