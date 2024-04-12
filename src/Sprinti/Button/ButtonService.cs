using Iot.Device.Button;

namespace Sprinti.Button;

public class ButtonService(ButtonBase button, ILogger<ButtonService> logger) : IButtonService
{
    public Task WaitForSignalAsync(CancellationToken cancellationToken)
    {
        var pressed = false;
        button.Press += (_, _) => { pressed = true; };
        while (!cancellationToken.IsCancellationRequested)
        {
            if (!pressed) continue;
            logger.LogInformation("Button pressed");
            return Task.CompletedTask;
        }

        logger.LogError("No button pressed in timeout interval");
        throw new TimeoutException("Timeout reached: No button pressed in timeout interval");
    }
}