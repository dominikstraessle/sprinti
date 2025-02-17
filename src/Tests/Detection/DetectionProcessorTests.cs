using OpenCvSharp;
using Sprinti.Detection;
using Sprinti.Domain;

namespace Sprinti.Tests.Detection;

public class DetectionProcessorTests(IDetectionProcessor processor)
{
    [Fact(Skip = "error 127")]
    public void TryDetectCubesTest()
    {
        var invalidPath = DetectionTestFiles.GetImagePath("invalid.png");
        using var invalidImageHsv = Cv2.ImRead(invalidPath);
        Cv2.CvtColor(invalidImageHsv, invalidImageHsv, ColorConversionCodes.BGR2HSV);
        Assert.False(processor.TryDetectCubes(invalidImageHsv, out var config));
        Assert.Null(config);

        var imagePath1 = DetectionTestFiles.GetDetectionFileName("20240523083949.png");
        using var imageHsv1 = Cv2.ImRead(imagePath1);
        Cv2.CvtColor(imageHsv1, imageHsv1, ColorConversionCodes.BGR2HSV);

        var imagePath2 = DetectionTestFiles.GetDetectionFileName("20240523084050.png");
        using var imageHsv2 = Cv2.ImRead(imagePath2);
        Cv2.CvtColor(imageHsv2, imageHsv2, ColorConversionCodes.BGR2HSV);

        Assert.False(processor.TryDetectCubes(imageHsv1, out config));
        Assert.Null(config);

        Assert.True(processor.TryDetectCubes(imageHsv2, out config));
        Assert.NotNull(config);

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

        Assert.Equal(expected, config.Config);
        Assert.True(DateTime.Now.Ticks >= config.Time.Ticks);
    }
}