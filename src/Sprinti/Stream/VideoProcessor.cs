using Microsoft.Extensions.Options;
using OpenCvSharp;
using Sprinti.Domain;

namespace Sprinti.Stream;

public interface IVideoProcessor
{
    CubeConfig? Run(CancellationToken stoppingToken);
}

public class VideoProcessor(
    IStreamCapture capture,
    IDetectionProcessor processor,
    ILogger<VideoStream> logger,
    IOptions<StreamOptions> options) : IVideoProcessor
{
    public CubeConfig? Run(CancellationToken stoppingToken)
    {
        using var image = new Mat();
        while (!stoppingToken.IsCancellationRequested)
        {
            if (!capture.Read(image))
            {
                logger.LogInformation("Failed to read image from stream");
            }

            logger.LogInformation("Received image: {rows}x{cols}", image.Rows, image.Cols);

            if (processor.TryDetectCubes(image, out var config))
            {
                logger.LogInformation("Detected config: {config}", config);
                return config;
            }

            logger.LogInformation("Could not complete detection yet.");
        }

        logger.LogInformation("Detection could not be completed before cancellation was requested");
        return null;
    }
}