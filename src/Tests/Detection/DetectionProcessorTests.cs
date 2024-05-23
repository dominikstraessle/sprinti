using OpenCvSharp;
using Sprinti.Detection;
using Sprinti.Domain;

namespace Sprinti.Tests.Detection;

public class DetectionProcessorTests(IDetectionProcessor processor)
{
    [Fact]
    public void TryDetectCubesTest()
    {
        var imagePath1 = TestFiles.GetDetectionFileName("20240517093113.png");
        using var imageHsv1 = Cv2.ImRead(imagePath1);
        Cv2.CvtColor(imageHsv1, imageHsv1, ColorConversionCodes.BGR2HSV);

        var imagePath2 = TestFiles.GetDetectionFileName("20240517093300.png");
        using var imageHsv2 = Cv2.ImRead(imagePath2);
        Cv2.CvtColor(imageHsv2, imageHsv2, ColorConversionCodes.BGR2HSV);

        Assert.False(processor.TryDetectCubes(imageHsv1, out var config));
        Assert.Null(config);

        Assert.True(processor.TryDetectCubes(imageHsv2, out config));
        Assert.NotNull(config);

        var expected = new SortedDictionary<int, Color>
        {
            { 1, Color.Red },
            { 2, Color.Blue },
            { 3, Color.None },
            { 4, Color.Red },
            { 5, Color.Yellow },
            { 6, Color.None },
            { 7, Color.None },
            { 8, Color.None }
        };

        Assert.Equal(expected, config.Config);
        Assert.True(DateTime.Now.Ticks >= config.Time.Ticks);
    }
}