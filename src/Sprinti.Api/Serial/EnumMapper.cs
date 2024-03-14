namespace Sprinti.Api.Serial;

public static class EnumMapper
{
    public enum Color
    {
        Red = 0,
        Blue = 1,
        Yellow = 2
    }

    public enum Direction
    {
        Up = 0,
        Down = 1
    }

    public enum ResponseState
    {
        Complete,
        InvalidArgument,
        NotImplemented,
        MachineError,
        Error,
        Unknown,
        Finished
    }


    public static string Map(this Color color) => color switch
    {
        Color.Red => "red",
        Color.Blue => "blue",
        Color.Yellow => "yellow",
        _ => throw new ArgumentOutOfRangeException(nameof(color), color, null)
    };

    public static string Map(this Direction direction) => direction switch
    {
        Direction.Up => "up",
        Direction.Down => "down",
        _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
    };
}