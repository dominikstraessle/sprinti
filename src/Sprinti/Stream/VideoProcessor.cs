using Microsoft.Extensions.Options;
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
    ILogger<VideoProcessor> logger,
    IOptions<StreamOptions> options,
    IHostEnvironment environment
) : IVideoProcessor
{
    public CubeConfig? RunDetection(CancellationToken stoppingToken)
    {
        var imageDirectory = Path.Combine(environment.ContentRootPath, options.Value.DebugPathFromContentRoot, $"{DateTime.Now:yyyyMMddHHmmss}");
        if (options.Value.Debug)
        {
            logger.LogInformation("Create debug path: {Path}", imageDirectory);
            Directory.CreateDirectory(imageDirectory);
        }

        logger.LogInformation("Start video processing: Checking for valid images.");
        using var imageHsv = new Mat();
        while (!stoppingToken.IsCancellationRequested)
        {
            if (!capture.Read(imageHsv))
            {
                logger.LogWarning("Failed to read image from stream");
            }

            Cv2.CvtColor(imageHsv, imageHsv, ColorConversionCodes.BGR2HSV);

            logger.LogTrace("Received image: {Rows}x{Cols}", imageHsv.Rows, imageHsv.Cols);

            string? debugDirectory = null;
            if (options.Value.Debug)
            {
                debugDirectory = Path.Combine(imageDirectory, $"{DateTime.Now:yyyyMMddHHmmss}");
            }

            if (processor.TryDetectCubes(imageHsv, out var config, debugDirectory))
            {
                logger.LogInformation("Config detected at {Time}: {Config}", config.Time, config.Config);
                return config;
            }

            logger.LogTrace("Could not complete detection yet.");
        }

        logger.LogWarning("Detection could not be completed before cancellation was requested");
        return null;
    }
}