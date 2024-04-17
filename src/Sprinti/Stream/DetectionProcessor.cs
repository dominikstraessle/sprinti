using System.Diagnostics.CodeAnalysis;
using OpenCvSharp;
using Sprinti.Domain;

namespace Sprinti.Stream;

public class DetectionProcessor(ImageSelector selector, CubeDetector detector, ILogger<DetectionProcessor> logger)
{
    private readonly int[][] _result = InitResult();

    public bool TryDetectCubes(Mat image, [MaybeNullWhen(false)] out CubeConfig config)
    {
        config = null;
        if (!selector.TrySelectImage(image, out var lookupConfig))
        {
            logger.LogInformation("No image selected");
            return IsCompleteResult(_result);
        }

        detector.DetectCubes(image, lookupConfig.LookupTable, _result);

        if (!IsCompleteResult(_result))
        {
            logger.LogInformation("Result not complete after detection: {Result}", _result.ToString());
            return false;
        }

        config = new CubeConfig
        {
            Time = DateTime.Now,
            Config = ResultToConfig(_result)
        };
        logger.LogInformation("Result complete after detection: {Result}", _result.ToString());
        logger.LogInformation("Config detected: {Config}", config);
        return true;
    }

    internal static int[][] InitResult()
    {
        var result = new int[8][];
        for (var i = 0; i < result.Length; i++)
        {
            result[i] = new int[4];
        }

        return result;
    }


    internal static bool IsCompleteResult(IEnumerable<int[]> result)
    {
        return result.Select(ints => ints.Max()).All(i => i > 0);
    }

    internal static SortedDictionary<int, Color> ResultToConfig(int[][] result)
    {
        var config = new SortedDictionary<int, Color>();
        for (var i = 0; i < result.Length; i++)
        {
            var r = result[i];
            var maxElement = r.Max();

            var maxIndex = Array.IndexOf(r, maxElement);
            config.Add(i + 1, (Color)maxIndex);
        }

        return config;
    }
}