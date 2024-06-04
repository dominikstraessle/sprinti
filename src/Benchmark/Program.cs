using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sprinti.Detection;

namespace Benchmark;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        ConfigureBuilder(builder);

        var app = builder.Build();

        await app.Services.GetRequiredService<BenchmarkService>().BenchmarkAsync(new CancellationToken());
    }


    private static void ConfigureBuilder(HostApplicationBuilder builder)
    {
        builder.Configuration.AddJsonFile("/home/dominik/aworkspace/study/pren/sprinti/src/Benchmark/detection.json",
            true, true);
        builder.Services.AddStreamModule(builder.Configuration);
        builder.Services.AddSingleton<BenchmarkService>();
    }
}