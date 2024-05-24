using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenCvSharp;
using Sprinti.Detection;
using Sprinti.Domain;

namespace Sprinti.Tests.Detection;

public class VideoProcessorTests
{
    private readonly VideoProcessor _processor;

    public VideoProcessorTests(IDetectionProcessor detectionProcessor, ILogger<VideoProcessor> logger,
        IOptions<StreamOptions> streamOptions, IHostEnvironment environment)
    {
        var testStreamCapture = new TestStreamCapture([
            DetectionTestFiles.GetDetectionFileName("20240523083949.png"),
            DetectionTestFiles.GetDetectionFileName("20240523084050.png")
        ]);
        _processor = new VideoProcessor(testStreamCapture, detectionProcessor, logger, streamOptions, environment);
    }

    [Fact]
    public void RunDetectionTest()
    {
        using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(2));
        var expected = new SortedDictionary<int, Color>
        {
            { 1, Color.Red },
            { 2, Color.Blue },
            { 3, Color.Red },
            { 4, Color.Red },
            { 5, Color.Blue },
            { 6, Color.Yellow },
            { 7, Color.None },
            { 8, Color.None }
        };

        var cubeConfig = _processor.RunDetection(cancellationTokenSource.Token);
        Assert.NotNull(cubeConfig);
        Assert.Equal(expected, cubeConfig.Config);
    }

    [Fact]
    public void RunDetectionTimeoutTest()
    {
        var cubeConfig = _processor.RunDetection(new CancellationToken(true));
        Assert.Null(cubeConfig);
    }

    internal class TestStreamCapture(IReadOnlyList<string> files) : IStreamCapture
    {
        private int _index;

        public bool Read(Mat mat)
        {
            if (_index >= files.Count) throw new ArgumentException("All files already processed");

            using var image = Cv2.ImRead(files[_index]);
            image.CopyTo(mat);
            _index++;
            return true;
        }
    }
}