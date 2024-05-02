using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenCvSharp;
using Sprinti.Stream;

namespace Calibrator;

public class CalibrationService(
    IStreamCapture capture,
    ILogger<CalibrationService> logger,
    IOptions<CaptureOptions> options,
    IHostEnvironment environment)
    : BackgroundService
{
    private CancellationToken _stoppingToken;

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Started Reader");
        return Task.Run(() => { CaptureFrames(stoppingToken); }, stoppingToken);
    }

    private void CaptureFrames(CancellationToken stoppingToken)
    {
        _stoppingToken = stoppingToken;
        var imageDirectory = Path.Combine(environment.ContentRootPath, options.Value.ImagePathFromContentRoot);
        Directory.CreateDirectory(imageDirectory);
        logger.LogInformation("Image directory: {Path}", imageDirectory);

        using var image = new Mat();
        var frameCount = 0;

        while (!stoppingToken.IsCancellationRequested)
        {
            capture.Read(image);

            if (frameCount++ % options.Value.CaptureIntervalInFrames != 0) continue;
            var imageFilePath = Path.Combine(imageDirectory, $"{DateTime.Now:yyyyMMddHHmmss}.png");
            image.SaveImage(imageFilePath);
            logger.LogInformation("Received image: {Rows}x{Cols}, saved to {Path}", image.Rows, image.Cols,
                imageFilePath);
            new WindowService(imageFilePath, image).Calibrate();
        }
    }


    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Stopping Calibration Service");
        Cv2.DestroyAllWindows();
        await base.StopAsync(cancellationToken);
        logger.LogInformation("Stopped Calibration Service");
    }
}