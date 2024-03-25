using Sprinti.Domain;

namespace Sprinti.Api.Button
{
    public interface IConfirmationAdapter
    {
        Task<bool> TestConnection(HttpClient httpClient);
        void StartAsync(CancellationToken cancellation);
        void EndAysnc(CancellationToken cancellation);
        void Confirmation(CubeConfig cubeConfig);

    }
}
