using Microsoft.Extensions.Options;

namespace Sprinti.Api.Serial;

public class SerialService(ISerialAdapter serialAdapter, IOptions<SerialOptions> options, ILogger<SerialService> logger)
{
    private TimeSpan Timeout => TimeSpan.FromMilliseconds(options.Value.ReadTimeoutInMilliseconds);

    public async Task SendCommand(ISerialCommand command, CancellationToken cancellationToken)
    {
        var message = await CommandReply(command, cancellationToken);
    }

    public async Task<FinishedResponse> SendFinish(FinishCommand command, CancellationToken cancellationToken)
    {
        var message = await CommandReply(command, cancellationToken);
        return new FinishedResponse(10);
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