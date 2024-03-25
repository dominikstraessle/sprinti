using System.Globalization;
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

    public async Task StartAsync(CancellationToken cancellation)
    {
        logger.LogInformation("Send start to pren server.");
        await client.GetAsync(_connectionOptions.StartPath, cancellation);
        logger.LogInformation("Successfully completed start request to pren server");
    }


    public async Task Confirmation(CubeConfig cubeConfig)
    {
        cubeConfig.Time = DateTime.ParseExact(cubeConfig.Time.ToString(CultureInfo.InvariantCulture),
            "", CultureInfo.InvariantCulture);
        var jsonString = JsonSerializer.Serialize(cubeConfig);
    }

    public static string SerializeCubeConfig(CubeConfig cubeConfig)
    {
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        jsonSerializerOptions.Converters.Add(new JsonConverters("yyyy-mm-dd HH:mm:ss"));
        jsonSerializerOptions.Converters.Add(new CustomColorConverter());

        return JsonSerializer.Serialize(cubeConfig, jsonSerializerOptions);
    }
}