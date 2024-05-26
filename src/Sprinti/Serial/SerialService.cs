using Microsoft.Extensions.Options;
using Sprinti.Domain;

namespace Sprinti.Serial;

public interface ISerialService
{
    Task<CompletedResponse> SendCommand(ISerialCommand command, CancellationToken cancellationToken);
    Task<FinishedResponse> SendCommand(FinishCommand command, CancellationToken cancellationToken);
    Task<string> SendRawCommand(string command, CancellationToken stoppingToken);

    Task<double> RunWorkflowProcedure(IEnumerable<ISerialCommand> instructions, CancellationToken cancellationToken);
    Task RunStartProcedure(CancellationToken cancellationToken);
}

public class SerialService(
    ISerialAdapter serialAdapter,
    IOptions<SerialOptions> options,
    ILogger<SerialService> logger)
    : ISerialService
{
    private TimeSpan Timeout => TimeSpan.FromMilliseconds(options.Value.ReadTimeoutInMilliseconds);

    public async Task<CompletedResponse> SendCommand(ISerialCommand command, CancellationToken cancellationToken)
    {
        var message = await CommandReply(command, cancellationToken);
        var responseState = ParseResponseState(message);
        return new CompletedResponse(responseState);
    }

    public async Task<FinishedResponse> SendCommand(FinishCommand command, CancellationToken cancellationToken)
    {
        var message = await CommandReply(command, cancellationToken);
        var responseState = ParseResponseState(message);
        var powerInWatts = -1;
        if (responseState is ResponseState.Finished) powerInWatts = GetPowerInWatts(message);

        return new FinishedResponse(powerInWatts, responseState);
    }

    public async Task<string> SendRawCommand(string command, CancellationToken stoppingToken)
    {
        var readTask = Task.Run(() => ReadResponse(stoppingToken), stoppingToken);

        serialAdapter.WriteLine(command);

        return await readTask.WaitAsync(Timeout, stoppingToken);
    }

    public async Task<double> RunWorkflowProcedure(IEnumerable<ISerialCommand> instructions,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Instructions: {instructions}", instructions);

        await SendCommand(new StartCommand(), cancellationToken);
        foreach (var command in instructions)
        {
            await SendCommand(command, cancellationToken);
            await Task.Delay(TimeSpan.FromMilliseconds(options.Value.CommandDelayInMilliseconds), cancellationToken);
        }

        await SendCommand(new LiftCommand(Direction.Down), cancellationToken);
        var finishedResponse = await SendCommand(new FinishCommand(), cancellationToken);
        return finishedResponse.PowerInWattHours;
    }

    public async Task RunStartProcedure(CancellationToken cancellationToken)
    {
        logger.LogInformation("Run start procedure");

        List<ISerialCommand> commands =
        [
            new MoveoutCommand(),
            new InitCommand(),
            new AlignCommand()
        ];

        foreach (var command in commands)
        {
            await SendCommand(command, cancellationToken);
            await Task.Delay(TimeSpan.FromMilliseconds(options.Value.CommandDelayInMilliseconds), cancellationToken);
        }
    }

    private static ResponseState ParseResponseState(string response)
    {
        return response switch
        {
            "error 0" => ResponseState.Complete,
            _ when IsFinished(response) => ResponseState.Finished,
            "error 1" => ResponseState.NotImplemented,
            "error 2" => ResponseState.Error,
            _ => ResponseState.Unknown
        };
    }

    private static bool IsFinished(string s)
    {
        var split = s.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (split is not ["finish", _]) return false;

        return int.TryParse(split[1], out var number) && number >= 1;
    }

    private static int GetPowerInWatts(string s)
    {
        var split = s.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (split is not ["finish", _]) throw new ArgumentException(s, nameof(s));

        if (int.TryParse(split[1], out var number) && number >= 1) return number;

        throw new ArgumentException(s, nameof(s));
    }


    private async Task<string> CommandReply(ISerialCommand command, CancellationToken stoppingToken)
    {
        var readTask = Task.Run(() => ReadResponse(stoppingToken), stoppingToken);

        serialAdapter.WriteLine(command.ToAsciiCommand());

        return await readTask.WaitAsync(Timeout, stoppingToken);
    }

    private string ReadResponse(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
            try
            {
                return serialAdapter.ReadLine();
            }
            catch (TimeoutException)
            {
                logger.LogTrace("No message received in timeout interval");
            }

        logger.LogTrace("No message received in timeout interval");
        throw new TimeoutException("Timeout reached: Reading was cancelled before a message was received");
    }
}