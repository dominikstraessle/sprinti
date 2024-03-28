using Sprinti.Serial;

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

    public static string Map(this Direction direction)
    {
        return direction switch
        {
            Direction.Up => "up",
            Direction.Down => "down",
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    public static string Map(this ResponseState responseState)
    {
        return responseState switch
        {
            ResponseState.Complete => "Complete",
            ResponseState.InvalidArgument => "InvalidArgument",
            ResponseState.NotImplemented => "NotImplemented",
            ResponseState.MachineError => "MachineError",
            ResponseState.Error => "Error",
            ResponseState.Unknown => "Unknown",
            ResponseState.Finished => "Finished",
            _ => throw new ArgumentOutOfRangeException(nameof(responseState), responseState, null)
        };
    }
}