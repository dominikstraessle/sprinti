using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Sprinti.Stream;

namespace Sprinti.Tests;

public abstract class Startup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        var jsonString = File.ReadAllText("/home/dominik/aworkspace/study/pren/sprinti/detection/config.json");

        var detectionOptions = JsonSerializer.Deserialize<DetectionOptions>(jsonString);
        if (detectionOptions is null)
        {
            throw new ArgumentException(nameof(DetectionOptions));
        }

        services.Configure<DetectionOptions>(options =>
        {
            options.LookupConfigs = detectionOptions.LookupConfigs;
        });
        services.AddTransient<ICubeDetector, CubeDetector>();
        services.AddTransient<IStreamCapture, StreamCapture>();
        services.AddTransient<IDetectionProcessor, DetectionProcessor>();
        services.AddTransient<IImageSelector, ImageSelector>();
        services.AddTransient<IVideoProcessor, VideoProcessor>();
    }
}