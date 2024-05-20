using System.Diagnostics.CodeAnalysis;
using OpenCvSharp;
using Sprinti.Domain;

namespace Sprinti.Detection;

public interface IDetectionProcessor
{
    bool TryDetectCubes(Mat imageHsv, [MaybeNullWhen(false)] out CubeConfig config, string? debug = null);
}

public class DetectionProcessor(IImageSelector selector, ICubeDetector detector, ILogger<DetectionProcessor> logger)
    : IDetectionProcessor
{
    private readonly int[][] _result = InitResult();

    public bool TryDetectCubes(Mat imageHsv, [MaybeNullWhen(false)] out CubeConfig config, string? debug)
    {
        config = null;
        if (!selector.TrySelectImage(imageHsv, out var lookupConfig, debug))
        {
            logger.LogTrace("No image selected");
            return IsCompleteResult(_result);
        }

        detector.DetectCubes(imageHsv, lookupConfig, _result, debug);

        if (!IsCompleteResult(_result))
        {
            logger.LogInformation("Result not complete after detection: {Result}", ResultToConfig(_result));
            return false;
        }

        config = new CubeConfig
        {
            Time = DateTime.Now,
            Config = ResultToConfig(_result)
        };
        logger.LogInformation("Result complete after detection: {Result}", ResultToConfig(_result));
        logger.LogInformation("Config detected at {Time}: {Config}", config.Time, config.Config);
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