using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using static Sprinti.Serial.EnumMapper;

namespace Sprinti.Serial;

public class SerialService(ISerialAdapter serialAdapter, IOptions<SerialOptions> options, ILogger<SerialService> logger)
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
        return responseState is ResponseState.Finished
            ? new FinishedResponse(GetPowerInWatts(message), responseState)
            : new FinishedResponse(0, responseState);
    }

    private static ResponseState ParseResponseState(string response) => response switch
    {
        "complete" => ResponseState.Complete,
        _ when IsFinished(response) => ResponseState.Finished,
        "error invalid_argument" => ResponseState.InvalidArgument,
        "error not_implemented" => ResponseState.NotImplemented,
        "error machine_error" => ResponseState.MachineError,
        "error error" => ResponseState.Error,
        _ => ResponseState.Unknown
    };

    private static bool IsFinished(string s)
    {
        var splitted = s.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (splitted is not ["finish", _])
        {
            return false;
        }

        return int.TryParse(splitted[1], out var number) && number >= 1;
    }

    private static int GetPowerInWatts(string s)
    {
        var splitted = s.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (splitted is not ["finish", _])
        {
            throw new ArgumentException(nameof(s), s);
        }

        if (int.TryParse(splitted[1], out var number) && number >= 1)
        {
            return number;
        }

        throw new ArgumentException(nameof(s), s);
    }


    private async Task<string> CommandReply(ISerialCommand command, CancellationToken stoppingToken)
    {
        var readTask = Task.Run(() => ReadResponse(stoppingToken), stoppingToken);

        serialAdapter.WriteLine(command.ToAsciiCommand());

        var responseLine = await readTask.WaitAsync(Timeout, stoppingToken);
        logger.LogInformation("Received serial response: '{responseLine}'", responseLine);
        return responseLine;
    }

    private string ReadResponse(CancellationToken stoppingToken)
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

        throw new TimeoutException("Timeout reached: Reading was cancelled before a message was received");
    }
}