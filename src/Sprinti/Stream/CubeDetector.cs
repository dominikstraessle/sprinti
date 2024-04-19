using Microsoft.Extensions.Options;
using OpenCvSharp;
using Sprinti.Domain;

namespace Sprinti.Stream;

public interface ICubeDetector
{
    void DetectCubes(Mat image, LookupConfig config, int[][] result, bool show = false);
}

public class CubeDetector(IOptions<DetectionOptions> options, ILogger<CubeDetector> logger) : ICubeDetector
{
    public void DetectCubes(Mat image, LookupConfig config, int[][] result, bool show = false)
    {
        if (result.Length != 8)
        {
            throw new ArgumentOutOfRangeException(nameof(result), result, "Result Table must be of length 8");
        }

        var masks = GetColorMaskPairs(image);

        for (var i = 0; i < config.Points.Count(); i++)
        {
            var point = config.Points.ElementAt(i);
            foreach (var (color, mask) in masks)
            {
                var maskedPixel = mask.Get<byte>(point[1], point[0]);
                if (maskedPixel != 255) continue;
                if (show) Show(mask, point, color);
                var lookupPosition = config.Lookup.ElementAt(i);
                logger.LogInformation("Detected cube: {Color} {P} at {position}", color, point, lookupPosition);
                result[lookupPosition][(int)color]++;
            }
        }

        DisposeColorMasks(masks);
    }

    private static void Show(Mat mask, int[] point, Color color)
    {
        using var debug = new Mat();
        mask.CopyTo(debug);
        Cv2.Circle(debug, point[0], point[1], 10, 255, 10);
        Cv2.Circle(debug, point[0], point[1], 5, 0, 5);
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
}