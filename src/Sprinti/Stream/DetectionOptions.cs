namespace Sprinti.Stream;

public class DetectionOptions
{
    public const string Detection = "Detection";

    public static readonly LookupConfig[] DefaultLookupConfigs =
    [
        new LookupConfig(
            new SelectorPoints(
                [435, 108],
                [495, 269]
            ),
            [
                [330, 220],
                [255, 180],
                [245, 110],
                [325, 60],
                [390, 90],
                [385, 165]
            ],
            [3, 2, 6, 5, 4, 0]
        ),
        new LookupConfig(
            new SelectorPoints(
                [178, 292],
                [170, 115]
            ),
            [
                [330, 220],
                [255, 180],
                [245, 110],
                [325, 60],
                [390, 90],
                [385, 165]
            ],
            [1, 0, 4, 7, 6, 2]
        )
    ];

    public IEnumerable<LookupConfig> LookupConfigs { get; set; }
}

public record LookupConfig(
    SelectorPoints SelectorPoints,
    int[][] Points,
    IEnumerable<int> Lookup,
    string Filename = "");

public record SelectorPoints(int[] P1, int[] P2);