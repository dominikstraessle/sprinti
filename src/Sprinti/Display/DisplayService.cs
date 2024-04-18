namespace Sprinti.Display;

class DisplayService(ILogger<DisplayService> logger) : IDisplayService
{
    public Task UpdateProgress(int stepNumber, string text, CancellationToken cancellationToken)
    {
        logger.LogInformation("Display: {step} - {text}", stepNumber, text);
        return Task.CompletedTask;
    }
}