using Microsoft.Extensions.Options;
using OpenCvSharp;

namespace Sprinti.Stream;

public static class ModuleRegistry
{
    public static void AddStreamModule(this IServiceCollection services, ConfigurationManager configuration)
    {
        var detectionOptions = configuration.GetSection(DetectionOptions.Detection);
        var detectionOptionsValue = detectionOptions.Get<DetectionOptions>();
        if (detectionOptionsValue is null || !detectionOptionsValue.LookupConfigs.Any())
            throw new ArgumentException($"No detections provided: ${nameof(DetectionOptions)}");

        services.Configure<DetectionOptions>(detectionOptions);

        if (!ISprintiOptions.RegisterOptions<StreamOptions>(services, configuration, StreamOptions.Stream)) return;

        services.AddTransient<VideoCapture>(provider =>
        {
            var options = provider.GetRequiredService<IOptions<StreamOptions>>();
            var capture = new VideoCapture(options.Value.RtspSource);
            return capture;
        });
        services.AddTransient<IStreamCapture, StreamCapture>();
        services.AddTransient<IImageSelector, ImageSelector>();
        services.AddTransient<ICubeDetector, CubeDetector>();
        services.AddTransient<ILogicalCubeDetector, LogicalCubeDetector>();
        services.AddTransient<IDetectionProcessor, DetectionProcessor>();
        services.AddTransient<IVideoProcessor, VideoProcessor>();

        if (!ISprintiOptions.RegisterOptions<CaptureOptions>(services, configuration, StreamOptions.Stream)) return;
        services.AddHostedService<VideoStream>();
    }
}