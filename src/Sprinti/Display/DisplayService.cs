namespace Sprinti.Display;

public class DisplayService(ILogger<DisplayService> logger) : IDisplayService
{
    public Task Display(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}