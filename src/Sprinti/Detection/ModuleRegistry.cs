using Microsoft.Extensions.Options;
using OpenCvSharp;

namespace Sprinti.Detection;

public static class ModuleRegistry
{
    public static void AddStreamModule(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddSingleton(new DetectionOptions());

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

        if (!ISprintiOptions.RegisterOptions<CaptureOptions>(services, configuration, CaptureOptions.Capture)) return;
        services.AddHostedService<VideoStream>();
    }
}