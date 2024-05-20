namespace Sprinti.Detection;

public interface ILogicalCubeDetector
{
    void DetectCubes(int[][] result);
}

internal class LogicalCubeDetector(ILogger<LogicalCubeDetector> logger) : ILogicalCubeDetector
{
    private const int NoneIndex = 0;
    private const int SkipStep = 4;

    public void DetectCubes(int[][] result)
    {
        for (var i = 0; i < SkipStep; i++)
        {
            var x = result[i][NoneIndex];
            if (x <= 0) continue;
            var skippedIndex = i + SkipStep;
            logger.LogInformation("Logical Detection: {Index} is None, therefore {SkippedIndex} must be None", i,
                skippedIndex);
            result[skippedIndex][NoneIndex] += 1;
        }
    }
}