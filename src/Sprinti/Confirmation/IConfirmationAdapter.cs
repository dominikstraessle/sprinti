using Sprinti.Domain;

namespace Sprinti.Confirmation;

public interface IConfirmationAdapter
{
    Task<bool> TestConnection(HttpClient httpClient);
    Task StartAsync(CancellationToken cancellation);
    void EndAsync(CancellationToken cancellation);
    Task Confirmation(CubeConfig cubeConfig);

}