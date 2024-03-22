using OpenCvSharp;

namespace Sprinti.Api.Video;

public class VideoStream(ILogger<VideoStream> logger) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Started Reader");
        return Task.Run(() =>
        {
            var capture = new VideoCapture(0);
            using var window = new Window("Camera");
            using var image = new Mat();
            // When the movie playback reaches end, Mat.data becomes NULL.
            while (!stoppingToken.IsCancellationRequested)
            {
                capture.Read(image); // same as cvQueryFrame
            }
        }, stoppingToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Stopping SerialReaderService");
        await base.StopAsync(cancellationToken);
    }
}