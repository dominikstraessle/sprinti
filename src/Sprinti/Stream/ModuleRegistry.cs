using Microsoft.Extensions.Options;
using OpenCvSharp;

namespace Sprinti.Stream;

public static class ModuleRegistry
{
    public static void AddStreamModule(this IServiceCollection services)
    {
        services.AddTransient<ImageSelector>();
        services.AddTransient<VideoCapture>(provider =>
        {
            var options = provider.GetRequiredService<IOptions<StreamOptions>>();
            var capture = new VideoCapture(options.Value.RtspSource);
            return capture;
        });
        services.AddTransient<IStreamCapture, StreamCapture>();
        services.AddTransient<IImageSelector, ImageSelector>();
        services.AddTransient<ICubeDetector, CubeDetector>();
        services.AddTransient<IDetectionProcessor, DetectionProcessor>();
        services.AddTransient<IVideoProcessor, VideoProcessor>();
        services.AddHostedService<VideoStream>();
    }
}