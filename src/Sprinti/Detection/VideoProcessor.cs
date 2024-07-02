using Microsoft.Extensions.Options;
using OpenCvSharp;
using Sprinti.Domain;

namespace Sprinti.Detection;

public interface IVideoProcessor
{
    CubeConfig? RunDetection(CancellationToken stoppingToken);
}

public class VideoProcessor(
    IStreamCapture capture,
    IDetectionProcessor processor,
    ILogger<VideoProcessor> logger,
    IOptions<StreamOptions> options,
    IHostEnvironment environment,
    StreamCaptureFactory factory
) : IVideoProcessor
{
    public CubeConfig? RunDetection(CancellationToken stoppingToken)
    {
        string? debugDirectory = null;
        if (options.Value.Debug)
        {
            debugDirectory = Path.Combine(environment.ContentRootPath, options.Value.DebugPathFromContentRoot,
                $"{DateTime.Now:yyyyMMddHHmmss}");
            logger.LogInformation("Create debug path: {Path}", debugDirectory);
            Directory.CreateDirectory(debugDirectory);
        }

        logger.LogInformation("Start video processing: Checking for valid images.");
        while (!stoppingToken.IsCancellationRequested)
        {
            using var imageHsv = new Mat();
            if (!capture.Read(imageHsv) || imageHsv.Empty())
            {
                logger.LogError("Failed to read image from stream. Skip");
                Thread.Sleep(options.Value.ErrorTimeout);
                capture = factory.Create();
                logger.LogError("Created new stream capture");
                continue;
            }

            Cv2.CvtColor(imageHsv, imageHsv, ColorConversionCodes.BGR2HSV);

            logger.LogTrace("Received image: {Rows}x{Cols}", imageHsv.Rows, imageHsv.Cols);

            if (processor.TryDetectCubes(imageHsv, out var config, debugDirectory) && config is not null)
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