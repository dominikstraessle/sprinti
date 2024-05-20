using Microsoft.Extensions.Options;
using OpenCvSharp;

namespace Sprinti.Detection;

public class VideoStream(
    IStreamCapture capture,
    ILogger<VideoStream> logger,
    IOptions<CaptureOptions> options,
    IHostEnvironment environment)
    : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Started Reader");
        return Task.Run(() => { CaptureFrames(stoppingToken); }, stoppingToken);
    }

    private void CaptureFrames(CancellationToken stoppingToken)
    {
        var imageDirectory = Path.Combine(environment.ContentRootPath, options.Value.ImagePathFromContentRoot);
        Directory.CreateDirectory(imageDirectory);
        logger.LogInformation("Image directory: {Path}", imageDirectory);

        using var image = new Mat();
        var frameCount = 0;

        while (!stoppingToken.IsCancellationRequested)
        {
            capture.Read(image);

            if (image.Empty())
            {
                continue;
            }

            if (frameCount++ % options.Value.CaptureIntervalInFrames != 0) continue;
            var imageFilePath = Path.Combine(imageDirectory, $"{DateTime.Now:yyyyMMddHHmmss}.png");
            image.SaveImage(imageFilePath);
            logger.LogInformation("Received image: {Rows}x{Cols}, saved to {Path}", image.Rows, image.Cols,
                imageFilePath);
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Stopping Video Stream");
        await base.StopAsync(cancellationToken);
    }
}