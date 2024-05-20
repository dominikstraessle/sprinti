namespace Sprinti.Detection;

public class DetectionOptions
{
    public const string Detection = "Detection";

    public IEnumerable<LookupConfig> LookupConfigs { get; set; } = [];
}

public record LookupConfig(
    SelectorPoints SelectorPoints,
    int[][] Points,
    IEnumerable<int> Lookup,
    string Filename = "");

public record SelectorPoints(int[] P1, int[] P2);