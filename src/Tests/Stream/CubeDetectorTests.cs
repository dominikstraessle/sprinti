using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using OpenCvSharp;
using Sprinti.Domain;
using Sprinti.Stream;

namespace Sprinti.Tests.Stream;

public class CubeDetectorTests
{
    private readonly CubeDetector _cubeDetector;
    private readonly int[][] _result;

    public CubeDetectorTests()
    {
        var options = new OptionsWrapper<ImageOptions>(new ImageOptions());

        _cubeDetector = new CubeDetector(options, NullLogger<CubeDetector>.Instance);
        _result = DetectionProcessor.InitResult();
    }

    [Fact]
    public void DetectCubes1Test()
    {
        int[][] expected =
        [
            [1, 0, 0, 0],
            [0, 0, 0, 0],
            [0, 1, 0, 0],
            [0, 0, 1, 0],
            [1, 0, 0, 0],
            [0, 0, 1, 0],
            [0, 0, 0, 1],
            [0, 0, 0, 0]
        ];

        var imagePath = TestFiles.GetTestFileFullName("1.png");
        using var image = Cv2.ImRead(imagePath);

        var lookupTable = ImageOptions.DefaultLookupConfigs[0].LookupTable;
        _cubeDetector.DetectCubes(image, lookupTable, _result);
        Assert.NotNull(_result);
        Assert.Equal(expected, _result);
    }

    [Fact]
    public void DetectCubes2Test()
    {
        int[][] expected =
        [
            [1, 0, 0, 0],
            [0, 0, 0, 1],
            [0, 1, 0, 0],
            [0, 0, 0, 0],
            [1, 0, 0, 0],
            [0, 0, 0, 0],
            [0, 0, 0, 1],
            [1, 0, 0, 0]
        ];

        var imagePath = TestFiles.GetTestFileFullName("2.png");
        using var image = Cv2.ImRead(imagePath);

        var lookupTable = ImageOptions.DefaultLookupConfigs[1].LookupTable;
        _cubeDetector.DetectCubes(image, lookupTable, _result);
        Assert.NotNull(_result);
        Assert.Equal(expected, _result);
    }

    [Fact]
    public void DetectCubes4FullTest()
    {
        int[][] expected =
        [
            [0, 2, 0, 0],
            [0, 0, 1, 0],
            [0, 0, 0, 2],
            [0, 1, 0, 0],
            [2, 0, 0, 0],
            [0, 0, 1, 0],
            [2, 0, 0, 0],
            [1, 0, 0, 0]
        ];

        var imagePath1 = TestFiles.GetTestFileFullName("4.1.png");
        using var image1 = Cv2.ImRead(imagePath1);
        var lookupTable1 = ImageOptions.DefaultLookupConfigs[0].LookupTable;
        _cubeDetector.DetectCubes(image1, lookupTable1, _result);

        var imagePath2 = TestFiles.GetTestFileFullName("4.2.png");
        using var image2 = Cv2.ImRead(imagePath2);
        var lookupTable2 = ImageOptions.DefaultLookupConfigs[1].LookupTable;
        _cubeDetector.DetectCubes(image2, lookupTable2, _result);

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