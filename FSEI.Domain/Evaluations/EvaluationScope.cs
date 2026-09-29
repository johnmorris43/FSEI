namespace FSEI.Domain.Evaluations;

public class EvaluationScope
{
    public int EvaluationScopeId { get; set; }

    public required string Name { get; set; }
    
    public required string Code { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<SystemsDepthEvaluation> SystemsDepthEvaluations { get; set; } = [];
}