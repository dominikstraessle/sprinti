using Sprinti.Domain;

namespace Sprinti.Api.Button
{
    public interface IConfirmationAdapter
    {
        Task<string> StartAsync(CancellationToken cancellation);
        void Confirmation(CubeConfig cubeConfig);
    }
}
