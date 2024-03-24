namespace Sprinti.Api.Domain;

public class CubeConfig
{
    public required DateTime Time { get; set; }
    public required SortedDictionary<int, Color> Config { get; set; }
}