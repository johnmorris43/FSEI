using FSEI.Domain.Products;

namespace FSEI.Domain.Releases;

public class ProductRelease
{
    public int ProductReleaseId { get; set; }
    public int ProductId { get; set; }

    public required string Version { get; set; }

    private int? _majorVersion;

    public int? MajorVersion
    {
        get => _majorVersion;
        set
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(MajorVersion),
                    "Major version cannot be negative.");
            }

            _majorVersion = value;
        }
    }
    private int? _minorVersion;

    public int? MinorVersion
    {
        get => _minorVersion;
        set
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(MinorVersion),
                    "Minor version cannot be negative.");
            }

            _minorVersion = value;
        }
    }
    private int? _patchVersion;

    public int? PatchVersion
    {
        get => _patchVersion;
        set
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(PatchVersion),
                    "Patch version cannot be negative.");
            }

            _patchVersion = value;
        }
    }

    public string? BuildNumber { get; set; }

    public ReleaseChannel ReleaseChannel { get; set; }

    public DateTime? ReleaseDate { get; set; }

    public string? ReleaseNotes { get; set; }

    public required Product Product { get; set; }
    
    private int? _preReleaseNumber;

    public int? PreReleaseNumber
    {
        get => _preReleaseNumber;

        set
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(PreReleaseNumber),
                    "Pre-release number cannot be negative.");
            }

            _preReleaseNumber = value;
        }
    }
    
    public ReleaseStatus ReleaseStatus { get; set; } = ReleaseStatus.Unknown;
    
    public ICollection<ReleaseChange> Changes { get; set; }
        = new List<ReleaseChange>();
}   