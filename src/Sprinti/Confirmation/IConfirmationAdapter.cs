namespace Sprinti.Confirmation;

public interface IConfirmationAdapter
{
    Task StartAsync(CancellationToken cancellation);
}