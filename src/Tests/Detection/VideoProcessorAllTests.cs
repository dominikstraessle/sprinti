using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenCvSharp;
using Sprinti.Detection;
using static Sprinti.Tests.Detection.VideoProcessorTests;

namespace Sprinti.Tests.Detection;

public class VideoProcessorAllTests(
    IDetectionProcessor detectionProcessor,
    IImageSelector imageSelector,
    DetectionOptions options,
    IOptions<StreamOptions> streamOptions,
    IHostEnvironment environment,
    ILogger<VideoProcessor> logger,
    StreamCaptureFactory factory)
{
    private VideoProcessor GetProcessor(int testCase)
    {
        var files = DetectionTestFiles.GetConfigImages(testCase);
        var testStreamCapture = new TestStreamCapture(files);
        return new VideoProcessor(testStreamCapture, detectionProcessor, logger, streamOptions, environment, factory);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void TestConfigs(int testCase)
    {
        var processor = GetProcessor(testCase);
        using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(100));
        var expected = DetectionTestFiles.GetResult(testCase);

        var cubeConfig = processor.RunDetection(cancellationTokenSource.Token);
        Assert.NotNull(cubeConfig);
        foreach (var kv in expected)
            Assert.True(cubeConfig.Config.Contains(kv),
                $"Expected: {kv.ToString()} Got: {cubeConfig.Config.GetValueOrDefault(kv.Key)}");

        Assert.Equal(expected, cubeConfig.Config);
    }

    [Theory(Skip = "")]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void CleanConfigs(int testCase)
    {
        foreach (var configImage in DetectionTestFiles.GetConfigImages(testCase))
        {
            using var imageHsv = Cv2.ImRead(configImage);
            Cv2.CvtColor(imageHsv, imageHsv, ColorConversionCodes.BGR2HSV);
            if (!imageSelector.TrySelectImage(imageHsv, out _)) File.Delete(configImage);
        }

        Assert.True(true);
    }


    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void DebugConfigs(int testCase)
    {
        var debugFolder = DetectionTestFiles.GetDebugPath(testCase.ToString());
        Directory.CreateDirectory(debugFolder);
        foreach (var configImage in DetectionTestFiles.GetConfigImages(testCase))
        {
            using var imageHsv = Cv2.ImRead(configImage);
            Cv2.CvtColor(imageHsv, imageHsv, ColorConversionCodes.BGR2HSV);
            var debugFolderPerFile = Path.Combine(debugFolder, Path.GetFileName(configImage));
            Directory.CreateDirectory(debugFolderPerFile);
            detectionProcessor.TryDetectCubes(imageHsv, out _, debugFolderPerFile);
        }

        Assert.True(true);
    }

    [Theory]
    [InlineData(-13, 0)]
    public async void MovePoints(int x, int y)
    {
        foreach (var config in options.LookupConfigs)
        {
            foreach (var point in config.Points)
            {
                point[0] += x;
                point[1] += y;
            }

            config.SelectorPoints.P1[0] += x;
            config.SelectorPoints.P1[1] += y;
            config.SelectorPoints.P2[0] += x;
            config.SelectorPoints.P2[1] += y;
        }

        var updatedConfigJson = JsonSerializer.Serialize(options);
        await File.WriteAllTextAsync(DetectionTestFiles.GetDetectionFileName("new.json"), updatedConfigJson);
        Assert.True(true);
    }
}