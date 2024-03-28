using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using Sprinti.Domain;

namespace Sprinti.Confirmation;

public class ConfirmationAdapter : IConfirmationAdapter
{
    private readonly ConfirmationOptions _connectionOptions;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions();
    private readonly HttpClient _client;
    private readonly ILogger<ConfirmationAdapter> _logger;

    public ConfirmationAdapter(HttpClient client,
        IOptions<ConfirmationOptions> options,
        ILogger<ConfirmationAdapter> logger)
    {
        _client = client;
        _logger = logger;
        _connectionOptions = options.Value;
        _jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    }

    public async Task StartAsync(CancellationToken cancellation)
    {
        _logger.LogInformation("Send start to pren server.");
        await _client.PostAsync(_connectionOptions.CubesTeamStartPath, null, cancellation);
        _logger.LogInformation("Successfully completed start request to pren server");
    }

    public async Task EndAsync(CancellationToken cancellation)
    {
        _logger.LogInformation("Send end to pren server.");
        await _client.PostAsync(_connectionOptions.CubesTeamEndPath, null, cancellation);
        _logger.LogInformation("Successfully completed end request to pren server");
    }

    public async Task ConfirmAsync(CubeConfig config, CancellationToken cancellation)
    {
        _logger.LogInformation("Send confirm to pren server: {content}", config);
        await _client.PostAsJsonAsync(_connectionOptions.CubesTeamConfigPath, config, _jsonSerializerOptions,
            cancellation);
        _logger.LogInformation("Successfully completed confirm request to pren server");
    }

    public async Task<bool> HealthCheckAsync(CancellationToken cancellation)
    {
        _logger.LogInformation("Send health check to pren server.");
        try
        {
            var response = await _client.GetAsync(_connectionOptions.CubesPath, cancellation);
            _logger.LogInformation("Completed health check request to pren server: {responseCode}",
                response.StatusCode);
            return response.IsSuccessStatusCode;
        }
        catch (Exception e)
        {
            _logger.LogError("Error on health check to pren server: {exception}", e.Message);
        }

        return false;
    }
}