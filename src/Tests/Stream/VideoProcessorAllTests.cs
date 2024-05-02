using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using OpenCvSharp;
using Sprinti.Stream;
using static Sprinti.Tests.Stream.VideoProcessorTests;

namespace Sprinti.Tests.Stream;

public class VideoProcessorAllTests(IDetectionProcessor detectionProcessor, IImageSelector imageSelector, ILogger<VideoProcessor> logger)
{
    private VideoProcessor GetProcessor(int testCase)
    {
        var files = TestFiles.GetConfigImages(testCase);
        var testStreamCapture = new TestStreamCapture(files);
        return new VideoProcessor(testStreamCapture, detectionProcessor, logger);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    [InlineData(10)]
    [InlineData(11)]
    [InlineData(12)]
    public void TestConfigs(int testCase)
    {
        var processor = GetProcessor(testCase);
        using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(100));
        var expected = TestFiles.GetResult(testCase);

        var cubeConfig = processor.RunDetection(cancellationTokenSource.Token);
        Assert.NotNull(cubeConfig);
        foreach (var kv in expected)
        {
            Assert.True(cubeConfig.Config.Contains(kv),
                $"Expected: {kv.ToString()} Got: {cubeConfig.Config.GetValueOrDefault(kv.Key)}");
        }

        Assert.Equal(expected, cubeConfig.Config);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    [InlineData(10)]
    [InlineData(11)]
    [InlineData(12)]
    public void CleanConfigs(int testCase)
    {
        foreach (var configImage in TestFiles.GetConfigImages(testCase))
        {
            using var imageHsv = Cv2.ImRead(configImage);
            Cv2.CvtColor(imageHsv, imageHsv, ColorConversionCodes.BGR2HSV);
            if (!imageSelector.TrySelectImage(imageHsv, out _))
            {
                File.Delete(configImage);
            }
        }

        Assert.True(true);
    }
}