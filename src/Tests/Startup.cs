using System.Text.Json;
using Iot.Device.Graphics.SkiaSharpAdapter;
using Microsoft.Extensions.DependencyInjection;
using Sprinti.Detection;
using Sprinti.Display;

namespace Sprinti.Tests;

public abstract class Startup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging();

        var jsonString =
            File.ReadAllText(
                "/home/dominik/aworkspace/study/pren/sprinti/src/Sprinti/Detection/DetectionOptions/detection.json");

        var detectionJson = JsonSerializer.Deserialize<DetectionJson>(jsonString);
        if (detectionJson?.Detection.LookupConfigs is null) throw new ArgumentException(nameof(DetectionOptions));

        services.AddSingleton(detectionJson.Detection);
        services.AddTransient<ICubeDetector, CubeDetector>();
        services.AddTransient<ILogicalCubeDetector, LogicalCubeDetector>();
        services.AddTransient<StreamCaptureFactory>();
        services.AddTransient<IStreamCapture, StreamCapture>();
        services.AddTransient<IDetectionProcessor, DetectionProcessor>();
        services.AddTransient<IImageSelector, ImageSelector>();
        services.AddTransient<IVideoProcessor, VideoProcessor>();

        // https://github.com/dotnet/iot/issues/2181#issuecomment-1833238952
        SkiaSharpAdapter.Register();
        services.AddTransient<IRenderService, RenderService>();
    }
}

public class DetectionJson
{
    public DetectionOptions Detection { get; set; }
}