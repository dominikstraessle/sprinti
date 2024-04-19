using OpenCvSharp;
using Sprinti.Domain;

namespace Sprinti.Stream;

public interface IVideoProcessor
{
    CubeConfig? RunDetection(CancellationToken stoppingToken);
}

public class VideoProcessor(
    IStreamCapture capture,
    IDetectionProcessor processor,
    ILogger<VideoStream> logger
) : IVideoProcessor
{
    public CubeConfig? RunDetection(CancellationToken stoppingToken)
    {
        logger.LogInformation("Start video processing: Checking for valid images.");
        using var imageHsv = new Mat();
        while (!stoppingToken.IsCancellationRequested)
        {
            if (!capture.Read(imageHsv))
            {
                logger.LogWarning("Failed to read image from stream");
            }
            Cv2.CvtColor(imageHsv, imageHsv, ColorConversionCodes.BGR2HSV);

            logger.LogTrace("Received image: {rows}x{cols}", imageHsv.Rows, imageHsv.Cols);

            if (processor.TryDetectCubes(imageHsv, out var config))
            {
                logger.LogInformation("Config detected at {time}: {Config}", config.Time, config.Config);
                return config;
            }

            logger.LogTrace("Could not complete detection yet.");
        }

        logger.LogWarning("Detection could not be completed before cancellation was requested");
        return null;
    }
}