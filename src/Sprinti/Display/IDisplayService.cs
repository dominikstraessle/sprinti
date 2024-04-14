namespace Sprinti.Display;

public interface IDisplayService
{
    Task Display(CancellationToken cancellationToken);
}