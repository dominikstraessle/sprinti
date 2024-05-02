using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Sprinti.Stream;

namespace Sprinti.Tests;

public abstract class Startup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging();

        var jsonString = File.ReadAllText("/home/dominik/aworkspace/study/pren/sprinti/src/Sprinti/detection.json");

        var detectionOptions = JsonSerializer.Deserialize<DetectionJson>(jsonString);
        if (detectionOptions?.Detection.LookupConfigs is null)
        {
            throw new ArgumentException(nameof(DetectionOptions));
        }

        services.Configure<DetectionOptions>(options =>
        {
            options.LookupConfigs = detectionOptions.Detection.LookupConfigs;
        });
        services.AddTransient<ICubeDetector, CubeDetector>();
        services.AddTransient<ILogicalCubeDetector, LogicalCubeDetector>();
        services.AddTransient<IStreamCapture, StreamCapture>();
        services.AddTransient<IDetectionProcessor, DetectionProcessor>();
        services.AddTransient<IImageSelector, ImageSelector>();
        services.AddTransient<IVideoProcessor, VideoProcessor>();
    }
}

public class DetectionJson
{
    public DetectionOptions Detection { get; set; }
}