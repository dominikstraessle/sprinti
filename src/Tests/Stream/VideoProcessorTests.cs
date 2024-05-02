using Microsoft.Extensions.Logging;
using OpenCvSharp;
using Sprinti.Domain;
using Sprinti.Stream;

namespace Sprinti.Tests.Stream;

public class VideoProcessorTests
{
    private readonly VideoProcessor _processor;

    public VideoProcessorTests(IDetectionProcessor detectionProcessor, ILogger<VideoProcessor> logger)
    {
        var testStreamCapture = new TestStreamCapture([
            TestFiles.GetDetectionFileName("4.1.png"),
            TestFiles.GetDetectionFileName("4.2.png")
        ]);
        _processor = new VideoProcessor(testStreamCapture, detectionProcessor, logger);
    }

    [Fact]
    public void RunDetectionTest()
    {
        using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(2));
        var expected = new SortedDictionary<int, Color>
        {
            { 1, Color.Yellow },
            { 2, Color.Blue },
            { 3, Color.Red },
            { 4, Color.Yellow },
            { 5, Color.None },
            { 6, Color.Blue },
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
            if (_index >= files.Count)
            {
                throw new ArgumentException("All files already processed");
            }

            using var image = Cv2.ImRead(files[_index]);
            image.CopyTo(mat);
            _index++;
            return true;
        }
    }
}