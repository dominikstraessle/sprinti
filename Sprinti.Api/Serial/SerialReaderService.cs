namespace Sprinti.Api.Serial;

public class SerialReaderService(SerialService serialService, ILogger<SerialReaderService> logger)
    : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Started Reader");
        return Task.Run(async () =>
        {
            logger.LogInformation("Start reading");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var message = await serialService.SendCommand(new ResetCommand(), stoppingToken);
                    logger.LogInformation("Message received: {Message}", message);
                }
                catch (TimeoutException e)
                {
                    logger.LogError("Timeout: {e}", e);
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
        serialService.Dispose();
        base.Dispose();
    }
}