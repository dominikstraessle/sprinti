namespace Sprinti.Tests.Stream;

public static class TestFiles
{
    private static string TestPath => Path.GetDirectoryName(typeof(TestFiles).Assembly.Location) ??
                                      throw new InvalidOperationException();

    public static string GetTestFileFullName(string fileName)
    {
        return Path.Combine(TestPath, "Stream", "Images", fileName);
    }
}