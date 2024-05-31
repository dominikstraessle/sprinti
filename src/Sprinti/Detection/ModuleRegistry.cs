using Microsoft.Extensions.Options;
using OpenCvSharp;

namespace Sprinti.Detection;

public static class ModuleRegistry
{
    public static void AddStreamModule(this IServiceCollection services, ConfigurationManager configuration)
    {
        var detectionOptionsSection = configuration.GetSection(DetectionOptions.Detection);
        var detectionOptions = detectionOptionsSection.Get<DetectionOptions>();
        if (detectionOptions is null || !detectionOptions.LookupConfigs.Any())
            throw new ArgumentException($"No detections provided: ${nameof(DetectionOptions)}");
        services.AddSingleton(detectionOptions);


        if (!ISprintiOptions.RegisterOptions<StreamOptions>(services, configuration, StreamOptions.Stream)) return;

        services.AddSingleton<VideoCapture>(provider =>
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

        if (!ISprintiOptions.RegisterOptions<CaptureOptions>(services, configuration, CaptureOptions.Capture)) return;
        services.AddHostedService<VideoStream>();
    }
}