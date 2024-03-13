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

public class SerialService(ISerialAdapter serialAdapter, ILogger<SerialService> logger) : IDisposable
{
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(10);

    public async Task<string> SendCommand(ISerialCommand command, CancellationToken stoppingToken)
    {
        var readTask = Task.Run(() =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    return serialAdapter.ReadLine();
                }
                catch (TimeoutException)
                {
                    logger.LogTrace("No message received in timeout interval");
                }
            }

            return "";
        }, stoppingToken);

        serialAdapter.WriteLine(command.ToAsciiCommand());

        var responseLine = await readTask.WaitAsync(Timeout, stoppingToken);
        logger.LogInformation("Received serial response: '{responseLine}'", responseLine);
        return responseLine;
    }

    public void Dispose()
    {
        serialAdapter.Dispose();
    }
}