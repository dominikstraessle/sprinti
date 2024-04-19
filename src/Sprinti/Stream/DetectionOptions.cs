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
            new[]
            {
                new Point(330, 220),
                new Point(255, 180),
                new Point(245, 110),
                new Point(325, 60),
                new Point(390, 90),
                new Point(385, 165)
            },
            [3, 2, 6, 5, 4, 0]
        ),
        new LookupConfig(
            new SelectorPoints(
                [178, 292],
                [170, 115]
                ),
            new[]
            {
                new Point(330, 220),
                new Point(255, 180),
                new Point(245, 110),
                new Point(325, 60),
                new Point(390, 90),
                new Point(385, 165)
            },
            [1, 0, 4, 7, 6, 2]
        )
    ];

    public IEnumerable<LookupConfig> LookupConfigs { get; set; } = DefaultLookupConfigs;
}

public record LookupConfig(
    SelectorPoints SelectorPoints,
    IEnumerable<Point> Points,
    IEnumerable<int> Lookup);

public record SelectorPoints(int[] P1, int[] P2);

public record Point(int X, int Y);