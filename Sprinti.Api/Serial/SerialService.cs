namespace Sprinti.Api.Serial;

public class SerialService(ISerialAdapter serialAdapter, ILogger<SerialService> logger)
{
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(10);

    public async Task<string> SendCommand(ISerialCommand command, CancellationToken stoppingToken)
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