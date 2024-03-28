using Microsoft.Extensions.Options;
using OpenCvSharp;

namespace Sprinti.Stream;

public static class ModuleRegistry
{
    public static void AddStreamModule(this IServiceCollection services)
    {
        services.AddTransient<VideoCapture>(provider =>
        {
            var options = provider.GetRequiredService<IOptions<StreamOptions>>();
            var capture = new VideoCapture(options.Value.RtspSource);
            return capture;
        });
        services.AddHostedService<VideoStream>();
    }
}