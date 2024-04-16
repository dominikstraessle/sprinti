namespace Sprinti.Stream;

public class ImageOptions
{
    public const string Image = "Image";

    public static readonly Point[] DefaultPoints =
    [
        new Point(330, 220),
        new Point(255, 180),
        new Point(245, 110),
        new Point(325, 60),
        new Point(390, 90),
        new Point(385, 165)
    ];

    public static readonly LookupConfig[] DefaultLookupConfigs =
    [
        new LookupConfig(
            [3, 2, 6, 5, 4, 0],
            new Point(435, 108),
            new Point(495, 269)
        ),
        new LookupConfig(
            [1, 0, 4, 7, 6, 2],
            new Point(178, 292),
            new Point(170, 115)
        ),
    ];

    public IEnumerable<LookupConfig> LookupConfigs { get; set; } = DefaultLookupConfigs;
    public IEnumerable<Point> CubePoints { get; set; } = DefaultPoints;
}

public record LookupConfig(int[] LookupTable, Point P1, Point P2);

public record Point(int X, int Y);