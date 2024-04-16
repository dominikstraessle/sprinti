using Microsoft.Extensions.Options;
using OpenCvSharp;
using Sprinti.Domain;

namespace Sprinti.Stream;

public class CubeDetector(IOptions<ImageOptions> options, ILogger<CubeDetector> logger)
{
    public int[][] DetectCubes(Mat image, int[] lookupTable, bool show = false)
    {
        var points = options.Value.CubePoints.ToArray();
        if (lookupTable.Length != points.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(lookupTable), lookupTable,
                "Lookup Table must have same length as configured reference points");
        }

        var result = InitResult();

        var masks = GetColorMaskPairs(image);

        for (var i = 0; i < points.Length; i++)
        {
            var point = points[i];
            foreach (var (color, mask) in masks)
            {
                var maskedPixel = mask.Get<byte>(point.Y, point.X);
                if (show) Show(mask, point, color);
                if (maskedPixel != 255) continue;
                var lookupPosition = lookupTable[i];
                logger.LogInformation("Detected cube: {Color} {P} at {position}", color, point, lookupPosition);
                result[lookupPosition][(int)color]++;
            }
        }

        DisposeColorMasks(masks);

        return result;
    }

    private static void Show(Mat mask, Point point, Color color)
    {
        using var debug = new Mat();
        mask.CopyTo(debug);
        Cv2.Circle(debug, point.X, point.Y, 10, 255, 10);
        Cv2.Circle(debug, point.X, point.Y, 5, 0, 5);
        Cv2.ImShow(color.ToString(), debug);
        Cv2.WaitKey();
    }

    private static void DisposeColorMasks(Dictionary<Color, Mat> masks)
    {
        foreach (var mask in masks)
        {
            mask.Value.Dispose();
        }
    }

    private static Dictionary<Color, Mat> GetColorMaskPairs(Mat image)
    {
        return Enum.GetValues(typeof(Color)).Cast<Color>()
            .Select(color => new KeyValuePair<Color, Mat>(color, ImageMask.GetMask(color, image))).ToDictionary();
    }

    private static int[][] InitResult()
    {
        var result = new int[8][];
        for (var i = 0; i < result.Length; i++)
        {
            result[i] = new int[4];
        }

        return result;
    }
}