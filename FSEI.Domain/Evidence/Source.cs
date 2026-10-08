using FSEI.Domain.Aircraft;
using FSEI.Domain.Evaluations;

namespace FSEI.Domain.Evidence;

public class Source
{
    public int SourceId { get; set; }

    public int SourceTypeId { get; set; }

    public int EvidenceScopeId { get; set; }

    public int? AircraftId { get; set; }

    public int? VariantId { get; set; }

    public required string Name { get; set; }

    public string? Url { get; set; }

    public DateTime? DateAccessed { get; set; }

    public string? Notes { get; set; }

    public required SourceType SourceType { get; set; }

    public required EvidenceScope EvidenceScope { get; set; }

    public Aircraft.Aircraft? Aircraft { get; set; }

    public AircraftVariant? Variant { get; set; }
    
    public ICollection<EvaluationSource> SystemsDepthEvaluations { get; set; } = [];
    public ICollection<AvionicsEvaluationSource> AvionicsEvaluations { get; set; } = [];
}