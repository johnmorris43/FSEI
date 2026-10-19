namespace FSEI.Domain.Releases;

public class ReleaseChange
{
    public int Id { get; set; }

    public int ProductReleaseId { get; set; }

    public ProductRelease ProductRelease { get; set; } = null!;

    public ReleaseChangeType ChangeType { get; set; }
        = ReleaseChangeType.Unknown;

    public string Description { get; set; } = string.Empty;
}