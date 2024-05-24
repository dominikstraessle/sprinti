using OpenCvSharp;
using Sprinti.Detection;
using Sprinti.Domain;

namespace Sprinti.Tests.Detection;

public class CubeDetectorTests(ICubeDetector cubeDetector, DetectionOptions detectionOptions)
{
    private readonly int[][] _result = DetectionProcessor.InitResult();

    [Fact]
    public void DetectCubes1Test()
    {
        int[][] expected =
        [
            [0, 0, 0, 0],
            [0, 0, 1, 0],
            [0, 0, 0, 1],
            [0, 0, 0, 1],
            [0, 0, 1, 0],
            [0, 1, 0, 0],
            [0, 0, 0, 0],
            [1, 0, 0, 0]
        ];

        var imagePath = DetectionTestFiles.GetDetectionFileName("20240523083949.png");
        using var imageHsv = Cv2.ImRead(imagePath);
        Cv2.CvtColor(imageHsv, imageHsv, ColorConversionCodes.BGR2HSV);

        var lookupTable = detectionOptions.LookupConfigs.First(config => config.Filename.Equals("20240523083949.png"));
        cubeDetector.DetectCubes(imageHsv, lookupTable, _result);
        Assert.NotNull(_result);
        Assert.Equal(expected, _result);
    }

    [Fact]
    public void IsCompleteResultTest()
    {
        int[][] result =
        [
            [1, 0, 0, 0],
            [0, 0, 0, 1],
            [0, 1, 0, 0],
            [1, 0, 0, 0],
            [1, 0, 0, 0],
            [0, 0, 1, 0],
            [0, 0, 0, 1],
            [1, 0, 0, 0]
        ];
        var actual = DetectionProcessor.IsCompleteResult(result);
        Assert.True(actual);
    }

    [Fact]
    public void IsNotCompleteResultTest()
    {
        int[][] result =
        [
            [0, 0, 0, 0],
            [0, 0, 0, 1],
            [0, 1, 0, 0],
            [1, 0, 0, 0],
            [1, 0, 0, 0],
            [0, 0, 1, 0],
            [0, 0, 0, 1],
            [1, 0, 0, 0]
        ];
        var actual = DetectionProcessor.IsCompleteResult(result);
        Assert.False(actual);
    }

    [Fact]
    public void ResultToConfigTest()
    {
        int[][] result =
        [
            [1, 0, 0, 0],
            [0, 0, 0, 1],
            [0, 1, 0, 0],
            [1, 0, 0, 0],
            [1, 0, 0, 0],
            [0, 0, 1, 0],
            [0, 0, 0, 1],
            [1, 0, 0, 0]
        ];
        var expected = new SortedList<int, Color>
        {
            { 1, Color.None },
            { 2, Color.Red },
            { 3, Color.Yellow },
            { 4, Color.None },
            { 5, Color.None },
            { 6, Color.Blue },
            { 7, Color.Red },
            { 8, Color.None }
        };
        var actual = DetectionProcessor.ResultToConfig(result);
        Assert.Equal(expected, actual);
    }
}