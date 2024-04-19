using Microsoft.Extensions.Logging.Abstractions;
using OpenCvSharp;
using Sprinti.Domain;
using Sprinti.Stream;

namespace Sprinti.Tests.Stream;

public class VideoProcessorTests
{
    private readonly VideoProcessor _processor;

    public VideoProcessorTests(IDetectionProcessor detectionProcessor)
    {
        var testStreamCapture = new TestStreamCapture([
            "4.1.png",
            "4.2.png"
        ]);
        _processor = new VideoProcessor(testStreamCapture, detectionProcessor, NullLogger<VideoStream>.Instance);
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

    private class TestStreamCapture(IReadOnlyList<string> files) : IStreamCapture
    {
        private int _index;

        public bool Read(Mat mat)
        {
            var imagePath1 = TestFiles.GetDetectionFileName(files[_index]);
            using var image = Cv2.ImRead(imagePath1);
            image.CopyTo(mat);
            _index++;
            _index %= files.Count;
            return true;
        }
    }
}