using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using OpenCvSharp;
using Sprinti;
using Sprinti.Detection;

namespace Calibrator;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        ConfigureBuilder(builder);

        var app = builder.Build();

        Configure(app);

        app.Run();
    }

    private static void Configure(IHost app)
    {
    }

    private static void ConfigureBuilder(HostApplicationBuilder builder)
    {
        if (!ISprintiOptions.RegisterOptions<StreamOptions>(builder.Services, builder.Configuration,
                StreamOptions.Stream)) return;

        builder.Services.AddTransient<VideoCapture>(provider =>
        {
            var options = provider.GetRequiredService<IOptions<StreamOptions>>();
            var capture = new VideoCapture(options.Value.RtspSource);
            return capture;
        });
        builder.Services.AddTransient<IStreamCapture, StreamCapture>();
        builder.Services.AddHostedService<CalibrationService>();
    }
}