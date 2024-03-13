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
                    var response = await serialService.SendCommand(new ResetCommand(), stoppingToken);
                    logger.LogInformation("Message received: {response}", response);
                }
                catch (TimeoutException e)
                {
                    logger.LogWarning("Timeout: {e}", e);
                }
                catch (Exception e)
                {
                    logger.LogError("Error: {e}", e);
                }
            }
        }, stoppingToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Stopping SerialReaderService");
        await base.StopAsync(cancellationToken);
    }
}