using Microsoft.Extensions.DependencyInjection;
using Sprinti.Stream;

namespace Sprinti.Tests;

public abstract class Startup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<ICubeDetector, CubeDetector>();
        services.AddTransient<IStreamCapture, StreamCapture>();
        services.AddTransient<IDetectionProcessor, DetectionProcessor>();
        services.AddTransient<IImageSelector, ImageSelector>();
        services.AddTransient<IVideoProcessor, VideoProcessor>();
    }
}