using OpenCvSharp;
using Sprinti.Domain;
using Sprinti.Stream;

namespace Sprinti.Tests.Stream;

public class DetectionProcessorTests(IDetectionProcessor processor)
{
    [Fact]
    public void TryDetectCubesTest()
    {
        var imagePath1 = TestFiles.GetTestFileFullName("4.1.png");
        using var image1 = Cv2.ImRead(imagePath1);
        var imagePath2 = TestFiles.GetTestFileFullName("4.2.png");
        using var image2 = Cv2.ImRead(imagePath2);

        Assert.False(processor.TryDetectCubes(image1, out var config));
        Assert.Null(config);

        Assert.True(processor.TryDetectCubes(image2, out config));
        Assert.NotNull(config);

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

        Assert.Equal(expected, config.Config);
    }
}