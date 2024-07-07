using Iot.Device.Button;

namespace Sprinti.Button;

public class ButtonService(ButtonBase button, ILogger<ButtonService> logger) : IButtonService
{
    public Task WaitForSignalAsync(CancellationToken cancellationToken)
    {
        var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        EventHandler<EventArgs>? handler = null;
        handler = (_, _) =>
        {
            tcs.TrySetResult(true);
            logger.LogInformation("Button pressed");
            button.Press -= handler;
        };
        button.Press += handler;
        cancellationToken.Register(() =>
        {
            tcs.TrySetException(new TimeoutException("Timeout reached: No button pressed in timeout interval"));
            logger.LogError("No button pressed in timeout interval");
            button.Press -= handler;
        });

        return tcs.Task;
    }
}