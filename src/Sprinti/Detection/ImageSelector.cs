using System.Diagnostics.CodeAnalysis;
using OpenCvSharp;

namespace Sprinti.Detection;

public interface IImageSelector
{
    bool TrySelectImage(Mat imageHsv, [MaybeNullWhen(false)] out LookupConfig lookupConfig, string? debug = null);
}

public class ImageSelector(DetectionOptions options, ILogger<ImageSelector> logger) : IImageSelector
{
    public bool TrySelectImage(Mat imageHsv, [MaybeNullWhen(false)] out LookupConfig lookupConfig, string? debug)
    {
        lookupConfig = null;
        using var mask = ImageMask.WhiteMask(imageHsv);

        foreach (var config in options.LookupConfigs)
        {
            var selectorPoints = config.SelectorPoints;
            var p1 = mask.Get<byte>(selectorPoints.P1[1], selectorPoints.P1[0]);
            var p2 = mask.Get<byte>(selectorPoints.P2[1], selectorPoints.P2[0]);
            if (p1 != selectorPoints.P1[2] || p2 != selectorPoints.P2[2]) continue;
            lookupConfig = config;
            logger.LogInformation("Image selected by points: {P1} and {P2}. Lookup Table is {Table}", selectorPoints.P1,
                selectorPoints.P2, config.Lookup);

            if (debug is null) return true;
            Debug(imageHsv, lookupConfig, debug, selectorPoints);
            DebugMask(mask, lookupConfig, debug);

            return true;
        }

        logger.LogTrace("No image selected.");
        return false;
    }

    private static void DebugMask(Mat mask, LookupConfig lookupConfig, string debug)
    {
        var fileName = Path.Combine(debug, lookupConfig.Filename, "selector.png");
        using var imageDebug = new Mat();
        mask.SaveImage(fileName);
    }

    private void Debug(Mat imageHsv, LookupConfig lookupConfig, string debug, SelectorPoints selectorPoints)
    {
        var path = Path.Combine(debug, lookupConfig.Filename);
        logger.LogTrace("Create debug image path: {Path}", path);
        Directory.CreateDirectory(path);

        using var imageDebug = new Mat();
        Cv2.CvtColor(imageHsv, imageDebug, ColorConversionCodes.HSV2BGR);
        Cv2.Circle(imageDebug, selectorPoints.P1[0], selectorPoints.P1[1], 10, 255, 10);
        Cv2.Circle(imageDebug, selectorPoints.P2[0], selectorPoints.P2[1], 10, 255, 10);
        var fileName = Path.Combine(path, "select.png");
        imageDebug.SaveImage(fileName);
    }
}