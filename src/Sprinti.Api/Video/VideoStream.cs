using OpenCvSharp;

namespace Sprinti.Api.Video;

public class VideoStream(ILogger<VideoStream> logger) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Started Reader");
        return Task.Run(() =>
        {
            using (var fs = new FileStorage())
            {
                fs.Write("iii", 123);
                fs.Write("ddd", Math.PI);
                using (var tempMat = new Mat("lenna.png"))
                {
                    fs.Write("mat", tempMat);
                }
            }
            var capture = new VideoCapture(0);
            using var image = new Mat();
            // When the movie playback reaches end, Mat.data becomes NULL.
            while (!stoppingToken.IsCancellationRequested)
            {
                capture.Read(image); // same as cvQueryFrame
                logger.LogInformation("Received image: {rows}x{cols}", image.Rows, image.Cols);
            }
        }, stoppingToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Stopping SerialReaderService");
        await base.StopAsync(cancellationToken);
    }
}