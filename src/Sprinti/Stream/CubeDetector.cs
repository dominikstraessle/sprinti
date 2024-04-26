using OpenCvSharp;
using Sprinti.Domain;

namespace Sprinti.Stream;

public interface ICubeDetector
{
    void DetectCubes(Mat imageHsv, LookupConfig config, int[][] result, bool show = false);
}

public class CubeDetector(ILogicalCubeDetector logicalCubeDetector, ILogger<CubeDetector> logger) : ICubeDetector
{
    public void DetectCubes(Mat imageHsv, LookupConfig config, int[][] result, bool show = false)
    {
        if (result.Length != 8)
        {
            throw new ArgumentOutOfRangeException(nameof(result), result, "Result Table must be of length 8");
        }

        var masks = GetColorMaskPairs(imageHsv);

        for (var i = 0; i < config.Points.Length; i++)
        {
            var point = config.Points[i];
            foreach (var (color, mask) in masks)
            {
                var maskedPixel = mask.Get<byte>(point[1], point[0]);
                if (maskedPixel != 255) continue;
                var lookupPosition = config.Lookup.ElementAt(i);
                if (show) Show(mask, point, color, lookupPosition, config.Filename);
                logger.LogInformation("[{key}] Detected cube: {Color} {P} at {position}", config.Filename, color, point,
                    lookupPosition);
                result[lookupPosition][(int)color]++;
            }
        }

        logicalCubeDetector.DetectCubes(result);

        DisposeColorMasks(masks);
    }

    private static void Show(Mat mask, IReadOnlyList<int> point, Color color, int lookupPosition, string filename)
    {
        using var debug = new Mat();
        mask.CopyTo(debug);
        Cv2.Circle(debug, point[0], point[1], 10, 255, 10);
        Cv2.Circle(debug, point[0], point[1], 5, 0, 5);
        var text = $"{filename}: {lookupPosition} - {color}";
        Cv2.ImShow(text, debug);
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