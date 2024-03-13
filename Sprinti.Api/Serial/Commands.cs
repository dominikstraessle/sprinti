namespace Sprinti.Api.Serial;

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

public record EjectCommand(EnumMapper.Color Color) : ISerialCommand
{
    public string ToAsciiCommand()
    {
        return $"eject {Color.Map()}";
    }
}

public record LiftCommand(EnumMapper.Direction Direction) : ISerialCommand
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