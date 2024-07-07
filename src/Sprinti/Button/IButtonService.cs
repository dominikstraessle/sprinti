namespace Sprinti.Button;

public interface IButtonService
{
    Task WaitForSignalAsync(CancellationToken cancellationToken);
}