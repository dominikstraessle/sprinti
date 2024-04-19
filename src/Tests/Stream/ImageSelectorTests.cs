using Microsoft.Extensions.Options;
using OpenCvSharp;
using Sprinti.Stream;

namespace Sprinti.Tests.Stream;

public class ImageSelectorTests(IImageSelector imageSelector, IOptions<DetectionOptions> options)
{
    [Fact]
    public void TrySelectAllTest()
    {
        foreach (var filename in options.Value.LookupConfigs.Select(l => l.Filename))
        {
            var imagePath = TestFiles.GetDetectionFileName(filename);
            using var image = Cv2.ImRead(imagePath);

            var result = imageSelector.TrySelectImage(image, out var config);
            Assert.True(result, $"Invalid for {filename}");
            Assert.NotNull(config);
            Assert.Contains(config, options.Value.LookupConfigs);
        }
    }

    [Fact]
    public void TrySelectImage1Test()
    {
        var imagePath = TestFiles.GetTestFileFullName("1.png");
        using var image = Cv2.ImRead(imagePath);

        var result = imageSelector.TrySelectImage(image, out var config);
        Assert.True(result);
        Assert.NotNull(config);
        Assert.Equivalent(DetectionOptions.DefaultLookupConfigs[0].Lookup, config.Lookup);
    }

    [Fact]
    public void TrySelectImage2Test()
    {
        var imagePath = TestFiles.GetTestFileFullName("2.png");
        using var image = Cv2.ImRead(imagePath);

        var result = imageSelector.TrySelectImage(image, out var config);
        Assert.True(result);
        Assert.NotNull(config);
        Assert.Equivalent(DetectionOptions.DefaultLookupConfigs[1].Lookup, config.Lookup);
    }

    [Fact]
    public void TrySelectImageFailedTest()
    {
        var imagePath = TestFiles.GetTestFileFullName("3.png");
        using var image = Cv2.ImRead(imagePath);

        var result = imageSelector.TrySelectImage(image, out var config);
        Assert.False(result);
        Assert.Null(config);
    }
}