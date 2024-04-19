using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;
using OpenCvSharp;

namespace Sprinti.Stream;

public interface IImageSelector
{
    bool TrySelectImage(Mat image, [MaybeNullWhen(false)] out LookupConfig lookupConfig);
}

public class ImageSelector(IOptions<DetectionOptions> options, ILogger<ImageSelector> logger) : IImageSelector
{
    public bool TrySelectImage(Mat image, [MaybeNullWhen(false)] out LookupConfig lookupConfig)
    {
        lookupConfig = null;
        using var mask = ImageMask.WhiteMask(image);
        foreach (var config in options.Value.LookupConfigs)
        {
            var selectorPoints = config.SelectorPoints;
            var p1 = mask.Get<byte>(selectorPoints.P1[1], selectorPoints.P1[0]);
            var p2 = mask.Get<byte>(selectorPoints.P2[1], selectorPoints.P2[0]);
            if (p1 != 255 || p2 != 255) continue;
            lookupConfig = config;
            logger.LogInformation("Image selected by points: {P1} and {P2}. Lookup Table is {Table}", selectorPoints.P1,
                selectorPoints.P2, config.Lookup);
            return true;
        }

        logger.LogTrace("No image selected.");
        return false;
    }
}