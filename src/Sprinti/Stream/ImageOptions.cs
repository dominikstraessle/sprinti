namespace Sprinti.Stream;

public class ImageOptions
{
    public const string Image = "Image";

    public static readonly LookupConfig[] DefaultLookupConfigs =
    [
        new LookupConfig(
            [3, 2, 6, 5, 4, 1],
            new Point(435, 108),
            new Point(495, 269)
        )
    ];

    public IEnumerable<LookupConfig> LookupConfigs { get; set; } = DefaultLookupConfigs;
}

public record LookupConfig(int[] LookupTable, Point P1, Point P2);

public record Point(int X, int Y);