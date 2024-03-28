using Microsoft.Extensions.Options;
using Sprinti.Domain;

namespace Sprinti.Confirmation;

public class ConfirmationAdapter(
    HttpClient client,
    IOptions<ConfirmationOptions> options,
    ILogger<ConfirmationAdapter> logger)
    : IConfirmationAdapter
{
    private readonly ConfirmationOptions _connectionOptions = options.Value;

    public async Task StartAsync(CancellationToken cancellation)
    {
        logger.LogInformation("Send start to pren server.");
        await client.PostAsync(_connectionOptions.CubesTeamStartPath, null, cancellation);
        logger.LogInformation("Successfully completed start request to pren server");
    }

    public async Task EndAsync(CancellationToken cancellation)
    {
        logger.LogInformation("Send end to pren server.");
        await client.PostAsync(_connectionOptions.CubesTeamEndPath, null, cancellation);
        logger.LogInformation("Successfully completed end request to pren server");
    }

    public async Task ConfirmAsync(CubeConfig config, CancellationToken cancellation)
    {
        logger.LogInformation("Send confirm to pren server: {content}", config);
        await client.PostAsJsonAsync(_connectionOptions.CubesTeamConfigPath, config,
            cancellation);
        logger.LogInformation("Successfully completed confirm request to pren server");
    }

    public async Task<bool> HealthCheckAsync(CancellationToken cancellation)
    {
        logger.LogInformation("Send health check to pren server.");
        try
        {
            var response = await client.GetAsync(_connectionOptions.CubesPath, cancellation);
            logger.LogInformation("Completed health check request to pren server: {responseCode}",
                response.StatusCode);
            return response.IsSuccessStatusCode;
        }
        catch (Exception e)
        {
            logger.LogError("Error on health check to pren server: {exception}", e.Message);
        }

        return false;
    }
}