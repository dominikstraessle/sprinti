using Sprinti.Detection;

namespace Sprinti.Tests.Detection;

public class LogicalCubeDetectorTests(ILogicalCubeDetector cubeDetector)
{
    [Fact]
    public void DetectCubes1Test()
    {
        int[][] result =
        [
            [1, 0, 0, 0],
            [0, 0, 0, 0],
            [1, 0, 0, 0],
            [0, 0, 0, 0],
            [0, 0, 0, 0],
            [0, 0, 0, 0],
            [0, 0, 0, 0],
            [0, 0, 0, 0]
        ];
        int[][] expected =
        [
            [1, 0, 0, 0],
            [0, 0, 0, 0],
            [1, 0, 0, 0],
            [0, 0, 0, 0],
            [1, 0, 0, 0],
            [0, 0, 0, 0],
            [1, 0, 0, 0],
            [0, 0, 0, 0]
        ];

        cubeDetector.DetectCubes(result);
        Assert.NotNull(result);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void DetectCubes2Test()
    {
        int[][] result =
        [
            [1, 0, 0, 0],
            [2, 0, 0, 0],
            [1, 0, 0, 0],
            [4, 0, 0, 0],
            [0, 0, 0, 0],
            [0, 0, 0, 0],
            [0, 0, 0, 0],
            [0, 0, 0, 0]
        ];
        int[][] expected =
        [
            [1, 0, 0, 0],
            [2, 0, 0, 0],
            [1, 0, 0, 0],
            [4, 0, 0, 0],
            [1, 0, 0, 0],
            [1, 0, 0, 0],
            [1, 0, 0, 0],
            [1, 0, 0, 0]
        ];

        cubeDetector.DetectCubes(result);
        Assert.NotNull(result);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void DetectCubes0Test()
    {
        int[][] result =
        [
            [0, 0, 0, 0],
            [0, 0, 0, 0],
            [0, 0, 0, 0],
            [0, 0, 0, 0],
            [0, 0, 0, 0],
            [0, 0, 0, 0],
            [0, 0, 0, 0],
            [0, 0, 0, 0]
        ];
        int[][] expected =
        [
            [0, 0, 0, 0],
            [0, 0, 0, 0],
            [0, 0, 0, 0],
            [0, 0, 0, 0],
            [0, 0, 0, 0],
            [0, 0, 0, 0],
            [0, 0, 0, 0],
            [0, 0, 0, 0]
        ];

        cubeDetector.DetectCubes(result);
        Assert.NotNull(result);
        Assert.Equal(expected, result);
    }
}