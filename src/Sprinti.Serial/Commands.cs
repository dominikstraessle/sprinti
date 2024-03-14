namespace Sprinti.Serial;

public interface ISerialCommand
{
    string ToAsciiCommand();
}

public record RotateCommand(int Degree) : ISerialCommand
{
    public string ToAsciiCommand()
    {
        return $"rotate {Degree}";
    }
}

public record EjectCommand(Color Color) : ISerialCommand
{
    public string ToAsciiCommand()
    {
        return $"eject {Color.Map()}";
    }
}

public record LiftCommand(Direction Direction) : ISerialCommand
{
    public string ToAsciiCommand()
    {
        return $"lift {Direction.Map()}";
    }
}

public record ResetCommand : ISerialCommand
{
    public string ToAsciiCommand()
    {
        return "reset";
    }
}

public record FinishCommand : ISerialCommand
{
    public string ToAsciiCommand()
    {
        return "finish";
    }
}

public record FinishedResponse(int PowerInWattHours, ResponseState ResponseState);

public record CompletedResponse(ResponseState ResponseState);