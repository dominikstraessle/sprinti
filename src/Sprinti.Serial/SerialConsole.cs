using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using static Sprinti.Serial.EnumMapper.Color;

namespace Sprinti.Serial;

public class SerialConsole(SerialService serialService, ILogger<SerialConsole> logger)
    : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Started Reader");
        return Task.Run(async () =>
        {
            logger.LogInformation("Start reading");
            while (!stoppingToken.IsCancellationRequested)
                try
                {
                    var response = await serialService.SendCommand(new EjectCommand(Red), stoppingToken);
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
        }, stoppingToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Stopping SerialReaderService");
        await base.StopAsync(cancellationToken);
    }
}