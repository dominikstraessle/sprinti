using Microsoft.Extensions.Options;
using OpenCvSharp;

namespace Sprinti.Stream;

public class VideoStream(
    VideoCapture capture,
    ImageSelector selector,
    ILogger<VideoStream> logger,
    IOptions<StreamOptions> options)
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
        var currentWorkingDir = Directory.GetCurrentDirectory();
        var imageDirectory = Path.Combine(currentWorkingDir, options.Value.SaveImagePathFromProjectRoot);
        Directory.CreateDirectory(imageDirectory);
        logger.LogInformation("Created new image directory: {path}", imageDirectory);

        using var image = new Mat();

        // When the movie playback reaches end, Mat.data becomes NULL.
        while (!stoppingToken.IsCancellationRequested)
        {
            capture.Read(image);
            if (_imageIndex % options.Value.CaptureIntervalInSeconds == 0)
            {
                var imageFilePath = Path.Combine(imageDirectory, $"{DateTime.Now.ToString("yyyyMMddHHmmss")}.png");

                selector.TrySelectImage(image, out _);

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