using FSEI.Domain.Releases;

namespace FSEI.Domain.Simulators;

public class SimulatorRelease
{
    public int SimulatorReleaseId { get; set; }

    public int SimulatorId { get; set; }

    public required string Version { get; set; }

    private int? _majorVersion;

    public int? MajorVersion
    {
        get => _majorVersion;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(
                    nameof(MajorVersion),
                    "Major version cannot be negative.");

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
                throw new ArgumentOutOfRangeException(
                    nameof(MinorVersion),
                    "Minor version cannot be negative.");

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
                throw new ArgumentOutOfRangeException(
                    nameof(PatchVersion),
                    "Patch version cannot be negative.");

            _patchVersion = value;
        }
    }

    public string? BuildNumber { get; set; }

    public ReleaseChannel ReleaseChannel { get; set; } = ReleaseChannel.Unknown;

    private int? _preReleaseNumber;

    public int? PreReleaseNumber
    {
        get => _preReleaseNumber;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(
                    nameof(PreReleaseNumber),
                    "Pre-release number cannot be negative.");

            _preReleaseNumber = value;
        }
    }

    public ReleaseStatus ReleaseStatus { get; set; } = ReleaseStatus.Unknown;

    public DateTime? ReleaseDate { get; set; }

    public string? Notes { get; set; }

    public required Simulator Simulator { get; set; }
}