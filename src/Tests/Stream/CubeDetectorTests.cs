using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using OpenCvSharp;
using Sprinti.Stream;

namespace Sprinti.Tests.Stream;

public class CubeDetectorTests
{
    private readonly CubeDetector _cubeDetector;

    public CubeDetectorTests()
    {
        var options = new OptionsWrapper<ImageOptions>(new ImageOptions());

        _cubeDetector = new CubeDetector(options, NullLogger<CubeDetector>.Instance);
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

        var imagePath = TestFiles.GetTestFileFullName("1.png"); // Replace "YourImage.jpg" with the actual file name
        var image = Cv2.ImRead(imagePath);

        var lookupTable = ImageOptions.DefaultLookupConfigs[0].LookupTable;
        var actual = _cubeDetector.DetectCubes(image, lookupTable);
        Assert.NotNull(actual);
        Assert.Equal(expected, actual);
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

        var imagePath = TestFiles.GetTestFileFullName("2.png"); // Replace "YourImage.jpg" with the actual file name
        var image = Cv2.ImRead(imagePath);

        var lookupTable = ImageOptions.DefaultLookupConfigs[1].LookupTable;
        var actual = _cubeDetector.DetectCubes(image, lookupTable);
        Assert.NotNull(actual);
        Assert.Equal(expected, actual);
    }
}