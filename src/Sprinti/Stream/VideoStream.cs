using Microsoft.Extensions.Options;
using OpenCvSharp;

namespace Sprinti.Stream;

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
        logger.LogInformation("Image directory: {path}", imageDirectory);

        using var image = new Mat();
        var frameCount = 0;

        while (!stoppingToken.IsCancellationRequested)
        {
            capture.Read(image);

            if (frameCount++ % options.Value.CaptureIntervalInFrames != 0) continue;
            var imageFilePath = Path.Combine(imageDirectory, $"{DateTime.Now.ToString("yyyyMMddHHmmss")}.png");
            image.SaveImage(imageFilePath);
            logger.LogInformation("Received image: {rows}x{cols}, saved to {path}", image.Rows, image.Cols,
                imageFilePath);
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Stopping SerialReaderService");
        await base.StopAsync(cancellationToken);
    }
}