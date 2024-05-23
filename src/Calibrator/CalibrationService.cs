using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenCvSharp;
using Sprinti.Detection;

namespace Calibrator;

public class CalibrationService(
    IStreamCapture capture,
    ILogger<CalibrationService> logger,
    IOptions<CaptureOptions> options,
    IHostEnvironment environment)
    : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Started Calibrator");
        return Task.Run(() => CaptureFrames(stoppingToken), stoppingToken);
    }

    private void CaptureFrames(CancellationToken stoppingToken)
    {
        var imageDirectory = Path.Combine(environment.ContentRootPath, options.Value.ImagePathFromContentRoot);
        Directory.CreateDirectory(imageDirectory);
        logger.LogInformation("Image directory: {Path}", imageDirectory);

        using var image = new Mat();
        var frameCount = 0;
        IList<LookupConfig> configs = [];

        while (!stoppingToken.IsCancellationRequested)
        {
            capture.Read(image);
            if (image.Empty()) continue;

            if (frameCount++ % options.Value.CaptureIntervalInFrames != 0) continue;
            var filename = $"{DateTime.Now:yyyyMMddHHmmss}.png";
            var imageFilePath = Path.Combine(imageDirectory, filename);
            image.SaveImage(imageFilePath);
            logger.LogInformation("Received image: {Rows}x{Cols}, saved to {Path}", image.Rows, image.Cols,
                imageFilePath);
            var windowService = new WindowService(filename, image);
            var addedConfig = windowService.Calibrate(stoppingToken);

            if (addedConfig is null) continue;

            configs.Add(addedConfig);

            logger.LogInformation("Add config: {Config}", addedConfig);
        }

        Cv2.DestroyAllWindows();
        var updatedDetection = JsonSerializer.Serialize(new DetectionOptions
        {
            LookupConfigs = configs
        });
        File.WriteAllText(Path.Combine(imageDirectory, $"{DateTime.Now:yyyyMMddHHmmss}.json"), updatedDetection);
    }


    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Stopping Calibration Service");
        Cv2.DestroyAllWindows();
        await base.StopAsync(cancellationToken);
        logger.LogInformation("Stopped Calibration Service");
    }
}