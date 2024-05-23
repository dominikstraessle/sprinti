using System.Text.Json;
using Sprinti.Domain;

namespace Sprinti.Tests.Detection;

public static class TestFiles
{
    // Define the base path at one place
    private const string BasePath = "/home/dominik/aworkspace/study/pren/sprinti/src/Tests/Detection/Images/";

    public static string GetTestFileFullName(string fileName)
    {
        // Reuse the base path
        return Path.Combine(BasePath, fileName);
    }

    public static string GetDetectionFileName(string fileName)
    {
        // Reuse the base path
        return Path.Combine(BasePath, "Detection", fileName);
    }

    public static IEnumerable<string> GetDetectionFiles()
    {
        return Directory.GetFiles(Path.Combine(BasePath, "Detection"), "*.png").OrderBy(s => s).ToArray();
    }

    public static string GetDebugPath(string fileName)
    {
        // Reuse the base path
        return Path.Combine(BasePath, "Detection", "Debug", fileName);
    }

    public static string[] GetConfigImages(int testCase)
    {
        // Reuse the base path
        var path = Path.Combine(BasePath, "Configs", testCase.ToString());
        return Directory.GetFiles(path, "*.png").OrderBy(s => s).ToArray();
    }

    public static SortedDictionary<int, Color> GetResult(int testCase)
    {
        var jsonString = File.ReadAllText(Path.Combine(BasePath, "Configs", testCase.ToString(), "result.json"));

        return JsonSerializer.Deserialize<SortedDictionary<int, Color>>(jsonString) ??
               throw new InvalidOperationException();
    }
}