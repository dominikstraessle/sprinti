using Microsoft.Extensions.Options;
using OpenCvSharp;

namespace Sprinti.Api.Video;

public class VideoStream(ILogger<VideoStream> logger, IOptions<StreamOptions> options) : BackgroundService
{
    private string Source => $"rtsp://{options.Value.Username}:{options.Value.Password}@{options.Value.Host}";
    private int _imageIndex;

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Started Reader");
        return Task.Run(() =>
        {
            var dateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            var currentWorkingDir = Directory.GetCurrentDirectory();
            var imageDirectory = Path.Combine(currentWorkingDir, options.Value.SaveImagePathFromProjectRoot, dateTime);
            Directory.CreateDirectory(imageDirectory);
            logger.LogInformation("Created new image directory: {path}", imageDirectory);

            var capture = new VideoCapture(Source);
            using var image = new Mat();

            // When the movie playback reaches end, Mat.data becomes NULL.
            while (!stoppingToken.IsCancellationRequested)
            {
                capture.Read(image);

                var imageFilePath = Path.Combine(imageDirectory, $"{_imageIndex++}.png");
                image.SaveImage(imageFilePath);
                logger.LogInformation("Received image: {rows}x{cols}, saved to {path}", image.Rows, image.Cols,
                    imageFilePath);
            }
        }, stoppingToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Stopping SerialReaderService");
        await base.StopAsync(cancellationToken);
    }
}