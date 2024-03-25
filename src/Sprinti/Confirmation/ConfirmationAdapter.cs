using System.Globalization;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Sprinti.Domain;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Sprinti.Confirmation;

public class ConfirmationAdapter(
    HttpClient client,
    IOptions<ConfirmationOptions> options,
    ILogger<ConfirmationAdapter> logger)
    : IConfirmationAdapter
{
    private readonly ConfirmationOptions _connectionOptions = options.Value;


    public async Task Confirmation(CubeConfig cubeConfig)
    {
        cubeConfig.Time = DateTime.ParseExact(cubeConfig.Time.ToString(CultureInfo.InvariantCulture),
            "", CultureInfo.InvariantCulture);
        var jsonString = JsonSerializer.Serialize(cubeConfig);
        await PostConfig(jsonString);
    }

    public static string SerializeCubeConfig(CubeConfig cubeConfig)
    {
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        jsonSerializerOptions.Converters.Add(new CustomDateTimeConverter("yyyy-mm-dd HH:mm:ss"));
        jsonSerializerOptions.Converters.Add(new CustomColorConverter());

        return JsonSerializer.Serialize(cubeConfig, jsonSerializerOptions);
    }

    private async Task PostConfig(string jsonString)
    {
        var Url = _connectionOptions + "/config";
        using var httpClient = new HttpClient();
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
        try
        {
            var response = await httpClient.PostAsync(Url, content);
            if (response.IsSuccessStatusCode)
            {
                logger.LogInformation("Successfully posted the config.");
                var responseBody = await response.Content.ReadAsStringAsync();
                logger.LogInformation("Response from pren server: {response}", responseBody);
            }
            else
            {
                logger.LogError("Failed to post the config. Status code: {status}", response.StatusCode);
            }
        }
        catch (HttpRequestException e)
        {
            logger.LogError("Failed to post config to pren server: {error}", e.Message);
            throw;
        }
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            const string url = "http://52.58.217.104:5000/cubes";
            var response = await client.PostAsync(url, null, cancellationToken);
            await response.Content.ReadAsStringAsync(cancellationToken);
        }
        catch (OperationCanceledException e)
        {
            logger.LogError("Failed to send start to pren server. Request was canceled: {message}", e.Message);
            throw;
        }
        catch (HttpRequestException e)
        {
            logger.LogError("Failed to send start to pren server: {message}", e.Message);
            throw;
        }
    }

    public void EndAsync(CancellationToken cancellation)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> TestConnection(HttpClient httpClient)
    {
        var url = _connectionOptions.TestConnectionString;
        try
        {
            using var response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
        }
        catch (HttpRequestException e)
        {
            logger.LogError("Health-Check of pren server failed: {message}", e.Message);
        }

        return false;
    }
}