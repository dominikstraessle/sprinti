namespace Sprinti.Domain;

public static class EnumMapper
{
    public static string Map(this Color color)
    {
        return color switch
        {
            Color.None => "",
            Color.Red => "red",
            Color.Blue => "blue",
            Color.Yellow => "yellow",
            _ => throw new ArgumentOutOfRangeException(nameof(color), color, null)
        };
    }

    public static Color Map(string color)
    {
        return color switch
        {
            "" => Color.None,
            "red" => Color.Red,
            "blue" => Color.Blue,
            "yellow" => Color.Yellow,
            _ => throw new ArgumentOutOfRangeException(nameof(color), color, null)
        };
    }

    public static string Map(this Direction direction)
    {
        return direction switch
        {
            Direction.Up => "up",
            Direction.Down => "down",
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }
}