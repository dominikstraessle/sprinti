using System.Diagnostics;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenCvSharp;
using Sprinti.Detection;
using Sprinti.Domain;

namespace Benchmark;

public class BenchmarkService(
    IServiceScopeFactory scopeFactory,
    ILogger<BenchmarkService> logger,
    IOptions<CaptureOptions> options,
    IHostEnvironment environment)
{
    public record Result(double Seconds, CubeConfig? Config);

    public record FullResult(double AverageInSeconds, int Detections, Result[] Results);

    public async Task BenchmarkAsync(CancellationToken stoppingToken)
    {
        var imageDirectory = Path.Combine(environment.ContentRootPath, options.Value.ImagePathFromContentRoot);
        Directory.CreateDirectory(imageDirectory);
        logger.LogInformation("Directory: {Path}", imageDirectory);

        var frameCount = 0;
        IList<Result> times = [];

        while (!stoppingToken.IsCancellationRequested && frameCount < 500)
        {
            try
            {
                var randomNumberGenerator = new Random();
                var delay = TimeSpan.FromSeconds(randomNumberGenerator.Next(0, 11));
                await Task.Delay(delay, stoppingToken);
                using var scope = scopeFactory.CreateScope();
                var processor = scope.ServiceProvider.GetRequiredService<IVideoProcessor>();
                var watch = new Stopwatch();
                watch.Start();
                var config = processor.RunDetection(stoppingToken);
                watch.Stop();
                times.Add(new Result(watch.Elapsed.TotalSeconds, config));
                frameCount++;
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error during benchmark");
            }
        }

        Cv2.DestroyAllWindows();
        var jsonOption = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        var allTimes = JsonSerializer.Serialize(new FullResult(
            times.Select(result => result.Seconds).Average(),
            frameCount,
            times.ToArray()
        ), jsonOption);
        await File.WriteAllTextAsync(Path.Combine(imageDirectory, $"{DateTime.Now:yyyyMMddHHmmss}.json"), allTimes,
            stoppingToken);
    }
}