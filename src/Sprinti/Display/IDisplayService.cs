namespace Sprinti.Display;

public interface IDisplayService
{
    Task UpdateProgress(int stepNumber, string text, CancellationToken cancellationToken);
}