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
    private int _imageIndex;

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Started Reader");
        return Task.Run(async () => { await CaptureFrames(stoppingToken); }, stoppingToken);
    }

    private async Task CaptureFrames(CancellationToken stoppingToken)
    {
        var imageDirectory = Path.Combine(environment.ContentRootPath, options.Value.ImagePathFromContentRoot);
        Directory.CreateDirectory(imageDirectory);
        logger.LogInformation("Image directory: {path}", imageDirectory);

        using var image = new Mat();

        // When the movie playback reaches end, Mat.data becomes NULL.
        while (!stoppingToken.IsCancellationRequested)
        {
            capture.Read(image);
            if (_imageIndex % options.Value.CaptureIntervalInSeconds == 0)
            {
                var imageFilePath = Path.Combine(imageDirectory, $"{DateTime.Now.ToString("yyyyMMddHHmmss")}.png");

                image.SaveImage(imageFilePath);
                logger.LogInformation("Received image: {rows}x{cols}, saved to {path}", image.Rows, image.Cols,
                    imageFilePath);
            }

            _imageIndex += 1;
            _imageIndex %= options.Value.CaptureIntervalInSeconds;
            await Task.Delay(1000, stoppingToken);
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Stopping SerialReaderService");
        await base.StopAsync(cancellationToken);
    }
}