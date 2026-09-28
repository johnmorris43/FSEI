namespace FSEI.Domain.Evidence;

public class EvidenceScope
{
    public int EvidenceScopeId { get; set; }

    public required string Name { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<Source> Sources { get; set; } = [];
}