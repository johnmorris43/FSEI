namespace FSEI.Domain.Evaluations;

public class EvaluationMethodology
{
    public int EvaluationMethodologyId { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;
    
    public ICollection<EvaluationMethodologyVersion> Versions { get; set; } = [];
}