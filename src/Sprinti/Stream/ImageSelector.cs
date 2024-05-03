using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;
using OpenCvSharp;

namespace Sprinti.Stream;

public interface IImageSelector
{
    bool TrySelectImage(Mat image, [MaybeNullWhen(false)] out LookupConfig lookupConfig, string? debug = null);
}

public class ImageSelector(DetectionOptions options, ILogger<ImageSelector> logger) : IImageSelector
{
    public bool TrySelectImage(Mat image, [MaybeNullWhen(false)] out LookupConfig lookupConfig, string? debug = null)
    {
        lookupConfig = null;
        using var mask = ImageMask.WhiteMask(image);
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

            Cv2.Circle(image, selectorPoints.P1[0], selectorPoints.P1[1], 10, 255, 10);
            Cv2.Circle(image, selectorPoints.P2[0], selectorPoints.P2[1], 10, 255, 10);
            var fileName = Path.Combine(debug, $"select-{lookupConfig.Filename}");
            image.SaveImage(fileName);

            return true;
        }

        logger.LogTrace("No image selected.");
        return false;
    }
}