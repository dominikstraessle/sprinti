using OpenCvSharp;
using Sprinti.Domain;

namespace Sprinti.Detection;

public interface ICubeDetector
{
    void DetectCubes(Mat imageHsv, LookupConfig config, int[][] result, string? debug = null);
}

public class CubeDetector(ILogicalCubeDetector logicalCubeDetector, ImageMask imageMask, ILogger<CubeDetector> logger)
    : ICubeDetector
{
    public void DetectCubes(Mat imageHsv, LookupConfig config, int[][] result, string? debug)
    {
        if (result.Length != 8)
            throw new ArgumentOutOfRangeException(nameof(result), result, "Result Table must be of length 8");

        var masks = GetColorMaskPairs(imageHsv);

        using var imageDebug = new Mat();
        if (debug is not null) Cv2.CvtColor(imageHsv, imageDebug, ColorConversionCodes.HSV2BGR);

        for (var i = 0; i < config.Points.Length; i++)
        {
            var point = config.Points[i];
            foreach (var (color, mask) in masks)
            {
                var maskedPixel = mask.Get<byte>(point[1], point[0]);
                if (maskedPixel != 255) continue;
                var lookupPosition = config.Lookup.ElementAt(i);
                logger.LogInformation("[{Key}] Detected cube: {Color} {P} at {Position}", config.Filename, color, point,
                    lookupPosition);
                result[lookupPosition][(int)color]++;

                if (debug is null) continue;
                Cv2.PutText(imageDebug, $"{color}: {lookupPosition}", new Point(point[0], point[1]),
                    HersheyFonts.HersheyTriplex, 1, new Scalar(0));
            }

            if (debug is null) continue;

            Cv2.Circle(imageDebug, point[0], point[1], 10, 255, 10);
        }

        if (debug is not null)
        {
            var fileName = Path.Combine(debug, config.Filename, "points.png");
            imageDebug.SaveImage(fileName);

            foreach (var mask in masks)
            {
                var maskFileName = Path.Combine(debug, config.Filename, $"{(int)mask.Key}-{mask.Key}.png");
                mask.Value.SaveImage(maskFileName);
            }
        }

        logicalCubeDetector.DetectCubes(result);

        DisposeColorMasks(masks);
    }

    private static void DisposeColorMasks(Dictionary<Color, Mat> masks)
    {
        foreach (var mask in masks) mask.Value.Dispose();
    }

    private Dictionary<Color, Mat> GetColorMaskPairs(Mat image)
    {
        return Enum.GetValues(typeof(Color)).Cast<Color>()
            .Select(color => new KeyValuePair<Color, Mat>(color, imageMask.GetMask(color, image))).ToDictionary();
    }
}