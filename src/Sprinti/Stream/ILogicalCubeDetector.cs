namespace Sprinti.Stream;

public interface ILogicalCubeDetector
{
    void DetectCubes(int[][] result);
}

internal class LogicalCubeDetector : ILogicalCubeDetector
{
    private const int NoneIndex = 0;
    private const int SkipStep = 4;

    public void DetectCubes(int[][] result)
    {
        for (var i = 0; i < SkipStep; i++)
        {
            var x = result[i][NoneIndex];
            if (x > 0)
            {
                result[i + SkipStep][NoneIndex] += 1;
            }
        }
    }
}