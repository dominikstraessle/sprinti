namespace Sprinti.Api.Serial;

public class SerialReaderService(ISerialAdapter serialAdapter, ILogger<SerialReaderService> logger)
    : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Started Reader");
        return Task.Run(() =>
        {
            logger.LogInformation("Start reading");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var message = serialAdapter.ReadLine();
                    logger.LogInformation("Message received: {Message}", message);
                }
                catch (TimeoutException)
                {
                    logger.LogTrace("No message received in timeout interval");
                }
            }
        }, stoppingToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Stopping SerialReaderService");
        await base.StopAsync(cancellationToken);
    }

    public override void Dispose()
    {
        serialAdapter.Dispose();
        base.Dispose();
    }
}